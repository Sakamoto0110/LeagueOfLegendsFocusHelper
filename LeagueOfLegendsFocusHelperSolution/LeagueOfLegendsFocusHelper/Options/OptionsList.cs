using System;

namespace LeagueOfLegendsFocusHelper.Options
{
    public class OptionsList
    {
        public delegate void OptionsChangedEventHandler(object sender, OptionsChangedEventArgs args);

        public event EventHandler OnOptionsChanged;

        public object Tag { get; private set; } = "UpdateOptions";
        private int _Time = 5;
        private bool _IsActive = true;
        private string _SoundName;
        private int _Volume;
        private bool _AutoPause;

        public void RaiseUpdate()
        {
            OnOptionsChanged?.Invoke(this, EventArgs.Empty);
        }

        //public OptionsList(int t, bool state, string sound, int vol)
        //{
        //    _Time = t;
        //    _IsActive = state;
        //    _SoundName = sound;
        //    _Volume = vol;
        //}

        public OptionsList(){}

        public OptionsList Clone()
        {
            return new OptionsList()
            {
                _Time = this._Time,
                _IsActive = this._IsActive,
                _SoundName = this._SoundName,
                _Volume = this._Volume,
                _AutoPause = this._AutoPause
            };
        }

        public int Time
        {
            get => _Time;

            set
            {
                if (value == _Time)
                    return;
                _Time = value;
                RaiseUpdate();
            }
        }

       
        public bool IsActive
        {
            get => _IsActive;

            set
            {
                if (value == _IsActive)
                    return;
                
                _IsActive = value;
                RaiseUpdate();
            }
        }


        public string SoundName
        {
            get => _SoundName;
            set
            {
                if (value == _SoundName)
                    return;
                _SoundName = value;
                RaiseUpdate();
            }
        }

        
        public int Volume
        {
            get => _Volume;
            set
            {
                if (value == _Volume)
                    return;
                _Volume = value;
                
                RaiseUpdate();
            }
        }

        public bool AutoPause
        {
            get => _AutoPause;
            set
            {
                if (value == _AutoPause)
                    return;
                _AutoPause = value;
                RaiseUpdate();
            }
        }

    }
}