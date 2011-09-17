using System.ComponentModel;
using GeniusCode.Components.Support.Reflection;
using NUnit.Framework;

namespace GeniusCode.Components.Core.Tests
{

    [TestFixture]
    public class NotifyPropertyChangedTests
    {

        private class Person : INotifyPropertyChanged
        {
            private string _status;
            public string Status
            {
                get { return _status; }
                set
                {
                    _status = value;
                    this.RaiseNotifyPropertyChanged(() => Status, PropertyChanged);
                }
            }
            #region Implementation of INotifyPropertyChanged

            public event PropertyChangedEventHandler PropertyChanged;

            #endregion
        }


        [Test]
        public void Test()
        {
            var person = new Person();
            var hapenned = false;
            person.PropertyChanged += (s, e) => hapenned = true;

            person.Status = "Hello, World";

            Assert.IsTrue(hapenned);
        }

    }




}
