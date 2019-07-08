using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Collections.Generic;
using JetBrains.Application.DataContext;
using JetBrains.Application.UI.Actions.ActionManager;
using JetBrains.Application.UI.ActionsRevised.Handlers;
using JetBrains.Application.UI.ActionsRevised.Loader;
using KaVE.RS.Commons.Settings;
using KaVE.VS.Commons;
using KaVE.VS.FeedbackGenerator.Generators;
using KaVE.VS.FeedbackGenerator.Menu;
using KaVE.VS.FeedbackGenerator.Settings.ExportSettingsSuite;
using KaVE.VS.FeedbackGenerator.Utils.Logging;
using Moq;
using NUnit.Framework;

namespace KaVE.VS.Dashboard.Tests
{
    [TestClass]
    public class DashboardCommandsTest
    {
        private Mock<ISettingsStore> _mockSettingsStore;
        private IUploadWizardWindowCreator _windowCreator;
        private Mock<ILogManager> _mockLogManager;

        private UploadWizardActionHandler _uut;
        private UserProfileSettings _userProfileSettings;
        private IUserProfileSettingsUtils _userProfileSettingsUtil;
        private ExportSettings _exportSettings;
        private IActionManager _am;

        [SetUp]
        public void Setup()
        {
            _userProfileSettings = new UserProfileSettings();
            _userProfileSettingsUtil = Mock.Of<IUserProfileSettingsUtils>();

            _mockSettingsStore = new Mock<ISettingsStore>();
            _windowCreator = Mock.Of<IUploadWizardWindowCreator>();
            _mockLogManager = new Mock<ILogManager>();

            _mockSettingsStore.Setup(settingStore => settingStore.GetSettings<UserProfileSettings>())
                              .Returns(_userProfileSettings);
            _exportSettings = new ExportSettings();
            _mockSettingsStore.Setup(settingStore => settingStore.GetSettings<ExportSettings>())
                              .Returns(_exportSettings);


            Registry.RegisterComponent(_userProfileSettingsUtil);
            Registry.RegisterComponent(_mockSettingsStore.Object);
            Registry.RegisterComponent(_windowCreator);
            Registry.RegisterComponent(_mockLogManager.Object);


            _am = MockActionManager();

            _uut = new UploadWizardActionHandler(
                _windowCreator,
                _userProfileSettingsUtil,
                _mockLogManager.Object,
                Mock.Of<IKaVECommandGenerator>(),
                _am);
        }

        private static IActionManager MockActionManager()
        {
            var am = Mock.Of<IActionManager>();
            var ad = Mock.Of<IActionDefs>();
            Mock.Get(am).SetupGet(a => a.Defs).Returns(ad);
            var ah = Mock.Of<IActionHandlers>();
            Mock.Get(am).SetupGet(a => a.Handlers).Returns(ah);
            return am;
        }

        [TestMethod]
        public void TestMethod1()
        {

        }

    }
}
