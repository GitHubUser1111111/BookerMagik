﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BookerMagikCore.Common.EventArguments;
using EntityLibrary.Abstract.Sport;
using EntityLibrary.Business.Sport.Football;

namespace BookerMagikCore.Bookmaker
{
    public abstract class BookmakerAbstract : IBookmaker
    {
        private Thread readLineThread;
        protected AutoResetEvent StopReadLineThreadEvent = new AutoResetEvent(false);

        protected abstract string ReadLineThreadName { get; }
        protected abstract object ReadLineThreadParameter { get; }
        protected abstract TimeSpan WaitStopReadThreadTimeout { get; }

        public event EventHandler<LineUpdatedEventArgs> LineUpdated;
        public abstract Task<bool> Login(string jsonConfiguration);
        public abstract Task<IEnumerable<FootballSportEvent>> ReadEvents();
        public abstract Task<IEnumerable<SportLeague>> ReadLeagues();
        public abstract Task<IEnumerable<SportType>> ReadSports();

        public void StartReadLineThread()
        {
            if (readLineThread != null) StopReadLineThread();

            readLineThread = new Thread(ReadLineFunction) {Name = ReadLineThreadName};
            readLineThread.SetApartmentState(ApartmentState.STA);
            readLineThread.IsBackground = true;
            readLineThread.Start(ReadLineThreadParameter);
        }

        public void StopReadLineThread()
        {
            if (readLineThread == null)
                return;

            StopReadLineThreadEvent.Set();
            if (!readLineThread.Join(WaitStopReadThreadTimeout)) readLineThread.Abort();

            readLineThread = null;
        }
        
        protected abstract void ReadLineFunction(object param);

        protected virtual void OnBookmakerLineChanged(IEnumerable<FootballSportEvent> sportEvents)
        {
            EventHandler<LineUpdatedEventArgs> handler = LineUpdated;
            LineUpdatedEventArgs args = new LineUpdatedEventArgs(sportEvents);
            handler?.Invoke(this, args);
        }
    }
}