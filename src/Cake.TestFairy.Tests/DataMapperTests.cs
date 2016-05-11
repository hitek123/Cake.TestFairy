using System.Collections.Specialized;
using Cake.TestFairy.Internal;
using Cake.TestFairy.Internal.Interfaces;
using FluentAssertions;
using NUnit.Framework;

namespace Cake.TestFairy.Tests
{
    public class DataMapperTests
    {
        private static readonly string _apikey = "apikey";
        private static readonly NameValueCollection _defaultValues = new NameValueCollection
        {
            { "api_key", _apikey },
            { "icon-watermark", "on" },
            { "video", "wifi" },
            { "max-duration", "10m" },
            { "comment", null },
            { "auto-update", "off" },
            { "notify", "on" },
            { "testers-groups", "" }
        };

        private IDataMapper _dataMapper;

        [SetUp]
        public void SetUp()
        {
            _dataMapper = new DataMapper();
        }

        [Test]
        public void MapSettings_HasProperDefaultValues()
        {
            //Arrange
            var settings = new TestFairyUploadSettings(_apikey);

            //Act
            NameValueCollection nameValueCollection = _dataMapper.MapSettings(settings);
            //Assert
            foreach (var nameValue in nameValueCollection.AllKeys)
                nameValueCollection[nameValue].Should().Be(_defaultValues[nameValue],
                    $"{nameValue} should have that value");
        }

        [Test]
        public void MapSettings_SetsAutoUpdateProperly_WhenSettingIsFalse()
        {
            //Arrange
            var settings = new TestFairyUploadSettings(_apikey) { AutoUpdate = false };

            //Act
            NameValueCollection nameValueCollection = _dataMapper.MapSettings(settings);
            
            //Assert
            nameValueCollection["auto-update"].Should().Be("off");
        }

        [Test]
        public void MapSettings_SetsAutoUpdateProperly_WhenSettingIsTrue()
        {
            //Arrange
            var settings = new TestFairyUploadSettings(_apikey) { AutoUpdate = true };

            //Act
            NameValueCollection nameValueCollection = _dataMapper.MapSettings(settings);
            
            //Assert
            nameValueCollection["auto-update"].Should().Be("on");
        }

        [Test]
        public void MapSettings_SetsNotifyProperly_WhenSettingIsFalse()
        {
            //Arrange
            var settings = new TestFairyUploadSettings(_apikey) { EmailTesters = false };

            //Act
            NameValueCollection nameValueCollection = _dataMapper.MapSettings(settings);
            
            //Assert
            nameValueCollection["notify"].Should().Be("off");
        }

        [Test]
        public void MapSettings_SetsNotifyProperly_WhenSettingIsTrue()
        {
            //Arrange
            var settings = new TestFairyUploadSettings(_apikey) { EmailTesters = true };

            //Act
            NameValueCollection nameValueCollection = _dataMapper.MapSettings(settings);
            
            //Assert
            nameValueCollection["notify"].Should().Be("on");
        }

        [Test]
        public void MapSettings_SetsIconWaterMarkProperly_WhenSettingIsFalse()
        {
            //Arrange
            var settings = new TestFairyUploadSettings(_apikey) { IconWaterMark = false };

            //Act
            NameValueCollection nameValueCollection = _dataMapper.MapSettings(settings);
            
            //Assert
            nameValueCollection["icon-watermark"].Should().Be("off");
        }

        [Test]
        public void MapSettings_SetsIconWaterMarkProperly_WhenSettingIsTrue()
        {
            //Arrange
            var settings = new TestFairyUploadSettings(_apikey) { IconWaterMark = true };

            //Act
            NameValueCollection nameValueCollection = _dataMapper.MapSettings(settings);
            
            //Assert
            nameValueCollection["icon-watermark"].Should().Be("on");
        }

        [Test]
        public void MapSettings_SetsVideoProperly_WhenSettingIsOff()
        {
            //Arrange
            var settings = new TestFairyUploadSettings(_apikey) { VideoRecording = VideoRecording.Off };

            //Act
            NameValueCollection nameValueCollection = _dataMapper.MapSettings(settings);
            
            //Assert
            nameValueCollection["video"].Should().Be("off");
        }

        [Test]
        public void MapSettings_SetsVideoProperly_WhenSettingIsWifi()
        {
            //Arrange
            var settings = new TestFairyUploadSettings(_apikey) { VideoRecording = VideoRecording.Wifi };

            //Act
            NameValueCollection nameValueCollection = _dataMapper.MapSettings(settings);
            
            //Assert
            nameValueCollection["video"].Should().Be("wifi");
        }

        [Test]
        public void MapSettings_SetsVideoProperly_WhenSettingIsOn()
        {
            //Arrange
            var settings = new TestFairyUploadSettings(_apikey) { VideoRecording = VideoRecording.On };

            //Act
            NameValueCollection nameValueCollection = _dataMapper.MapSettings(settings);
            
            //Assert
            nameValueCollection["video"].Should().Be("on");
        }

        [Test]
        public void MapSettings_SetsMaxDurationProperly_WhenSettingIs100()
        {
            //Arrange
            var settings = new TestFairyUploadSettings(_apikey) { MaxDurationMinutes = 100 };

            //Act
            NameValueCollection nameValueCollection = _dataMapper.MapSettings(settings);
            
            //Assert
            nameValueCollection["max-duration"].Should().Be("100m");
        }

        [Test]
        public void MapSettings_SetsCommentProperly_WhenSettingIsTest()
        {
            //Arrange
            var settings = new TestFairyUploadSettings(_apikey) { Comment = "Test" };

            //Act
            NameValueCollection nameValueCollection = _dataMapper.MapSettings(settings);
            
            //Assert
            nameValueCollection["comment"].Should().Be("Test");
        }

        [Test]
        public void MapSettings_SetsTesterGroupsProperly_WhenOneTesterGroup()
        {
            //Arrange
            var settings = new TestFairyUploadSettings(_apikey) { TesterGroups = new []{"One"} };

            //Act
            NameValueCollection nameValueCollection = _dataMapper.MapSettings(settings);
            
            //Assert
            nameValueCollection["testers-groups"].Should().Be("One");
        }

        [Test]
        public void MapSettings_SetsTesterGroupsProperly_WhenMultipleTesterGroups()
        {
            //Arrange
            var settings = new TestFairyUploadSettings(_apikey) { TesterGroups = new []{"One", "Two"} };

            //Act
            NameValueCollection nameValueCollection = _dataMapper.MapSettings(settings);
            
            //Assert
            nameValueCollection["testers-groups"].Should().Be("One,Two");
        }
    }
}