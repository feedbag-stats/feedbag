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

using KaVE.Commons.TestUtils.UserControls;
using KaVE.RS.Commons.Settings;
using KaVE.VS.FeedbackGenerator.Settings;
using KaVE.VS.FeedbackGenerator.UserControls.Anonymization;
using KaVE.VS.FeedbackGenerator.UserControls.UploadWizard.Anonymization;
using Moq;
using NUnit.Framework;

namespace KaVE.VS.FeedbackGenerator.Tests.UserControls.AnonymizationDialogs
{
    [RequiresSTA]
    internal class AnonymizationWindowTest : BaseUserControlTest
    {
        private Mock<ISettingsStore> _mockSettingsStore;

        [SetUp]
        public void SetUp()
        {
            _mockSettingsStore = new Mock<ISettingsStore>();
        }

        private AnonymizationWindow Open()
        {
            var anonymizationWindow = new AnonymizationWindow(_mockSettingsStore.Object);
            anonymizationWindow.Show();
            return anonymizationWindow;
        }

        [Test]
        public void DataContextIsSetCorrectly()
        {
            var sut = Open();
            Assert.IsInstanceOf<AnonymizationContext>(sut.DataContext);
        }

        [Test]
        public void ShouldSaveSettingsOnOkButton()
        {
            var sut = Open();

            UserControlTestUtils.Click(sut.OkButton);

            _mockSettingsStore.Verify(settingStore => settingStore.SetSettings(It.IsAny<AnonymizationSettings>()));
        }

        [Test]
        public void ShouldNotSaveSettingsOnCloseButton()
        {
            var sut = Open();

            UserControlTestUtils.Click(sut.CloseButton);

            _mockSettingsStore.Verify(settingStore => settingStore.SetSettings(It.IsAny<AnonymizationSettings>()), Times.Never);
        }
    }
}