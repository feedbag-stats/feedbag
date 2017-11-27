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

using JetBrains.Application;
using KaVE.VS.Commons;
using Moq;
using NUnit.Framework;

namespace KaVE.RS.Commons.Tests_Integration.Utils
{
    // TODO: move to commons solution, once the activation (ZoneMarker) issue is solved.
    internal class RegistryTest
    {
        [SetUp]
        public void SetUp()
        {
            Registry.Clear();
        }

        [Test]
        public void ShouldReturnShellComponent()
        {
            var component = Registry.GetComponent<ITestShellComponent>();

            Assert.IsTrue(component.IsReadDeal);
        }

        [Test]
        public void ShouldReturnMockComponent()
        {
            var testShellComponent = CreateMockComponent();

            Registry.RegisterComponent(testShellComponent);
            var component = Registry.GetComponent<ITestShellComponent>();

            Assert.IsFalse(component.IsReadDeal);
        }

        [Test]
        public void ShouldClearRegistrations()
        {
            var testShellComponent = CreateMockComponent();

            Registry.RegisterComponent(testShellComponent);
            Registry.Clear();
            var component = Registry.GetComponent<ITestShellComponent>();

            Assert.IsTrue(component.IsReadDeal);
        }

        [Test]
        public void RegistrationsCanBeChecked()
        {
            Assert.IsFalse(Registry.IsRegistered<ITestShellComponent>());
            Registry.RegisterComponent(CreateMockComponent());
            Assert.IsTrue(Registry.IsRegistered<ITestShellComponent>());
        }

        private static ITestShellComponent CreateMockComponent()
        {
            var mock = new Mock<ITestShellComponent>();
            mock.Setup(c => c.IsReadDeal).Returns(false);
            return mock.Object;
        }
    }

    public interface ITestShellComponent
    {
        bool IsReadDeal { get; }
    }

    [ShellComponent]
    public class TestShellComponent : ITestShellComponent
    {
        public bool IsReadDeal
        {
            get { return true; }
        }
    }
}