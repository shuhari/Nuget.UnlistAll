using System.ComponentModel;

namespace Nuget.UnlistAll.Models
{
    /// <summary>
    /// Package version item which is selectable from UI
    /// </summary>
    public class PackageVersion : INotifyPropertyChanged
    {
        public PackageVersion(string packageId, string version, bool selected)
        {
            this.PackageId = packageId;
            this.Version = version;
            this.Selected = selected;
        }

        private bool _selected = false;

        public string PackageId { get; private set; }

        public string Version { get; private set; }

        public bool Selected
        {
            get { return _selected; }
            set
            {
                if (_selected != value)
                {
                    _selected = value;
                    OnPropertyChanged(nameof(Selected));
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
