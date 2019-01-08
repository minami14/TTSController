using CeVIO.Talk.RemoteService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Speech
{
    class CeVIOController : IDisposable, ISpeechEngine
    {
        private string _libraryName;
        private bool _isActive = false;
        private Talker _talker;

        public SpeechEngineInfo Info { get; private set; }
        public string _text;

        public CeVIOController(SpeechEngineInfo info)
        {
            Info = info;
            var cevio = new CeVIOEnumerator();
            _libraryName = info.LibraryName;
        }

        public event EventHandler<EventArgs> Finished;
        protected virtual void OnFinished()
        {
            EventArgs se = new EventArgs();
            Finished?.Invoke(this, se);
        }

        public bool IsActive()
        {
            return _isActive;
        }

        public void Activate()
        {
            ServiceControl.StartHost(false);
            _talker = new Talker();
            _talker.Cast = _libraryName;
            _isActive = true;
        }

        public void Play(string text)
        {
            Task.Run(() =>
            {
                _talker.Speak(text).Wait();
                OnFinished();
            });
            _text = text;
        }

        public void Play()
        {
            Play(_text);
        }

        public void Stop()
        {
            _talker.Stop();
        }

        public void SetVolume(float value)
        {
            _talker.Volume = (uint)(value * 50);
        }

        public float GetVolume()
        {
            return _talker.Volume / 50f;
        }

        public void SetSpeed(float value)
        {
            _talker.Speed = (uint)(value * 25);
        }

        public float GetSpeed()
        {
            return _talker.Speed / 25f;
        }

        public void SetPitch(float value)
        {
            _talker.Tone = (uint)(value * 50);
        }

        public float GetPitch()
        {
            return _talker.Tone / 50f;
        }

        public void SetPitchRange(float value)
        {
            _talker.ToneScale = (uint)(value * 50);
        }

        public float GetPitchRange()
        {
            return _talker.ToneScale / 50f;
        }

        public void Dispose() { }
    }
}
