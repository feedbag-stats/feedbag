﻿/*
 * Copyright 2014 Technische Universität Darmstadt
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *    http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using JetBrains.Annotations;
using KaVE.Commons.Utils;
using KaVE.Commons.Utils.Assertion;
using KaVE.Commons.Utils.IO;
using KaVE.RS.Commons.Utils;
using KaVE.VS.Commons;
using Moq;
using NUnit.Framework;

namespace KaVE.RS.Commons.Tests_Unit.Utils.PublisherUtilsTestSuite
{
    [TestFixture]
    internal class UploadEventsByHttpTest
    {
        private const string FileContent = "arbitrary file content";
        private MemoryStream _stream;

        private static readonly Uri ValidUri = new Uri("http://server");

        private Mock<IIoUtils> _ioUtilsMock;
        private PublisherUtils _uut;

        [SetUp]
        public void SetUp()
        {
            _ioUtilsMock = new Mock<IIoUtils>();
            Registry.RegisterComponent(_ioUtilsMock.Object);
            _uut = new PublisherUtils();
            _stream = new MemoryStream();
        }

        [TearDown]
        public void CleanUpRegistry()
        {
            Registry.Clear();
        }

        [Test]
        public void ShouldInvokeTransferToCorrectUri()
        {
            var resp = CreateResponse(true);
            SetupResponse(resp);

            _uut.UploadEventsByHttp(_ioUtilsMock.Object, ValidUri, _stream);
            _ioUtilsMock.Verify(io => io.TransferByHttp(It.IsAny<HttpContent>(), ValidUri));
        }

        [Test]
        public void ShouldInvokeTransferWithCorrectData()
        {
            HttpContent lastUploadedContent = null;

            _ioUtilsMock.Setup(io => io.TransferByHttp(It.IsAny<HttpContent>(), ValidUri)).Returns(
                (HttpContent content, Uri uri) =>
                {
                    lastUploadedContent = content;
                    return CreateResponse(true);
                });

            _uut.UploadEventsByHttp(_ioUtilsMock.Object, ValidUri, new MemoryStream(FileContent.AsBytes()));

            var actual = ReadFirstContent(lastUploadedContent);
            Assert.AreEqual(FileContent, actual);
        }

        private const string TransferFailMessage = "Error XYZ";

        [Test, ExpectedException(typeof(AssertException), ExpectedMessage = TransferFailMessage)]
        public void ShouldFailIfTransferFails()
        {
            _ioUtilsMock.Setup(io => io.TransferByHttp(It.IsAny<HttpContent>(), ValidUri))
                        .Throws(new AssertException(TransferFailMessage));
            _uut.UploadEventsByHttp(_ioUtilsMock.Object, ValidUri, _stream);
        }

        [Test,
         ExpectedException(
             typeof(AssertException),
             ExpectedMessage = "Server response was empty")]
        public void ShouldFailIfMessageIsEmpty()
        {
            var resp = new HttpResponseMessage
            {
                Content = new StringContent("")
            };
            SetupResponse(resp);

            _uut.UploadEventsByHttp(_ioUtilsMock.Object, ValidUri, _stream);
        }

        [Test,
         ExpectedException(
             typeof(InvalidResponseException),
             ExpectedMessage = "Server response did not follow the expected format:\r\nXYZ")]
        public void ShouldFailIfMessageCannotBeParsed()
        {
            var resp = new HttpResponseMessage
            {
                Content = new StringContent("XYZ")
            };
            SetupResponse(resp);

            _uut.UploadEventsByHttp(_ioUtilsMock.Object, ValidUri, _stream);
        }

        [Test, ExpectedException(typeof(InvalidResponseException))]
        public void ShouldFailIfMessageCannotBeParsed_verifyLog()
        {
            var resp = new HttpResponseMessage
            {
                Content = new StringContent("XYZ")
            };
            SetupResponse(resp);

            _uut.UploadEventsByHttp(_ioUtilsMock.Object, ValidUri, _stream);
        }

        [Test,
         ExpectedException(
             typeof(AssertException),
             ExpectedMessage = "Server complains about invalid request:\r\nXYZ")]
        public void ShouldFailIfStateIsNotOk()
        {
            var resp = CreateResponse(false, "XYZ");
            SetupResponse(resp);

            _uut.UploadEventsByHttp(_ioUtilsMock.Object, ValidUri, _stream);
        }

        private void SetupResponse(HttpResponseMessage resp)
        {
            _ioUtilsMock.Setup(io => io.TransferByHttp(It.IsAny<HttpContent>(), ValidUri))
                        .Returns(resp);
        }

        private static HttpResponseMessage CreateResponse(bool isSuccessful, string message = null)
        {
            var state = isSuccessful ? "Ok" : "Fail";
            var responseMessage = "";
            if (message != null)
            {
                responseMessage = ",\"Message\": \"" + message + "\"";
            }

            var resp = new HttpResponseMessage
            {
                Content = new StringContent("{\"Status\": \"" + state + "\"" + responseMessage + "}")
            };
            return resp;
        }

        private static string ReadFirstContent([NotNull] HttpContent content)
        {
            // IsNotNull-Asserts are semantically equivalent to the corresponding IsInstanceOf-Assert but R# produces warnings otherwise
            // IsInstanceOf-Asserts are kept, because if they fail the result is more informative
            Assert.IsInstanceOf<MultipartFormDataContent>(content);
            var multipartFormDataContent = content as MultipartFormDataContent;
            Assert.IsNotNull(multipartFormDataContent);
            Assert.AreEqual(1, multipartFormDataContent.Count());
            var element = multipartFormDataContent.First();
            Assert.IsInstanceOf<ByteArrayContent>(element);
            var streamContent = element as ByteArrayContent;
            Assert.IsNotNull(streamContent);

            Assert.AreEqual("file", streamContent.Headers.ContentDisposition.Name);
            Assert.AreEqual("tmp.zip", streamContent.Headers.ContentDisposition.FileName);

            return streamContent.ReadAsByteArrayAsync().Result.AsString();
        }
    }
}