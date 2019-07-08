/*
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
using KaVE.Commons.Utils;
using KaVE.RS.Commons.Settings;
using Moq;
using NUnit.Framework;

namespace KaVE.RS.Commons.Tests_Unit.Settings
{
    internal class DashboardPrivacySettingsUtilsTest
    {
        private ISettingsStore _settingsStore;
        private DashboardPrivacySettings _settings;
        private IRandomizationUtils _rnd;
        private Guid _rndGuid;

        private DashboardPrivacySettingsUtils _sut;

        [SetUp]
        public void Setup()
        {
            _settings = new DashboardPrivacySettings();
            _settingsStore = Mock.Of<ISettingsStore>();
            Mock.Get(_settingsStore).Setup(ss => ss.GetSettings<DashboardPrivacySettings>()).Returns(_settings);

            _rndGuid = Guid.NewGuid();
            _rnd = Mock.Of<IRandomizationUtils>();
            Mock.Get(_rnd).Setup(r => r.GetRandomGuid()).Returns(_rndGuid);

            _sut = new DashboardPrivacySettingsUtils(_settingsStore, _rnd);
        }

        /*[Test]
        public void EnsureSharingDataEnabledCreatesInitialValue_false()
        {
            _settings.SharingDataEnabled = false;
            _sut.EnsureSharingDataEnabled();

            Assert.AreEqual(_rndGuid.ToString(), _settings.SharingDataEnabled);
            Mock.Get(_settingsStore).Verify(ss => ss.SetSettings(_settings));
        }

        [Test]
        public void EnsureIdCreatesInitialValue_true()
        {
            _settings.SharingDataEnabled = true;
            _sut.EnsureSharingDataEnabled();

            Assert.AreEqual(_rndGuid.ToString(), _settings.SharingDataEnabled);
            Mock.Get(_settingsStore).Verify(ss => ss.SetSettings(_settings));
        }*/

        [Test]
        public void EnsureSharingDataEnabledDoesNotOverwriteExistingValue()
        {
            _settings.SharingDataEnabled = true;
            _sut.EnsureSharingDataEnabled();

            Assert.AreEqual(true, _settings.SharingDataEnabled);
            Mock.Get(_settingsStore).Verify(ss => ss.SetSettings(_settings), Times.Never);
        }

        [Test]
        public void HasSharingDataEnabled()
        {
            Assert.False(_sut.HasSharingDataEnabled());
            _settings.SharingDataEnabled = true;
            Assert.True(_sut.HasSharingDataEnabled());
        }

        [Test]
        public void CreateProfile()
        {
            Assert.AreEqual(false, _sut.CreateNewSharingDataEnabled());
        }

        [Test]
        public void GetSettings()
        {
            var actual = _sut.GetSettings();
            Mock.Get(_settingsStore).Verify(ss => ss.GetSettings<DashboardPrivacySettings>());
            Assert.AreSame(_settings, actual);
        }

        [Test]
        public void StoreSettings()
        {
            _sut.StoreSettings(_settings);
            Mock.Get(_settingsStore).Verify(ss => ss.SetSettings(_settings));
        }
    }
}