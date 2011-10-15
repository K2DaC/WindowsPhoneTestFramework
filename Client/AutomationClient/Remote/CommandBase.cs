// ----------------------------------------------------------------------
// <copyright file="CommandBase.cs" company="Expensify">
//     (c) Copyright Expensify. http://www.expensify.com
//     This source is subject to the Microsoft Public License (Ms-PL)
//     Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//     All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
// ------------------------------------------------------------------------

using System;
using System.Threading;

namespace WindowsPhoneTestFramework.AutomationClient.Remote
{
    public partial class CommandBase
    {
        private bool _resultSent;

        public IConfiguration Configuration { get; set; }

        public void Do()
        {
            if (Configuration == null)
                throw new InvalidOperationException("Configuration must be set before Do() is called");

            try
            {
                DoImpl();
            }
            catch (Exception exception)
            {
                SendExceptionFailedResult(exception);
                if (exception is ThreadAbortException)
                    throw;
            }
        }

        protected virtual void DoImpl()
        {
            // base class DoImpl() should never be called.
            throw new InvalidOperationException("DoImpl should never be called in CommandBase");
        }

        protected void SendExceptionFailedResult(Exception exception)
        {
            var result = new ExceptionFailedResult()
                             {
                                 Id = Id,
                                 ExceptionMessage = exception.Message,
                                 ExceptionType = exception.GetType().FullName,
                                 FailureText = string.Format("Exception: {0}: {1}", exception.GetType().Name, exception.Message)
                             };
            result.Send(Configuration);
        }

        private void EnsureAtMostOneResultSent()
        {
            if (_resultSent)
            {
                // TODO - log this!
                throw new InvalidOperationException("Tried to send too many results");
            }

            _resultSent = true;
        }

        private void SendErrorResultIfNoOtherResultSent()
        {
            if (!_resultSent)
                SendExceptionFailedResult(
                    new InvalidOperationException("No result signalled by Command processing : " +
                                                  this.GetType().FullName));
        }

        protected void SkipResult()
        {
            EnsureAtMostOneResultSent();
        }

        protected void SendSuccessResult()
        {
            EnsureAtMostOneResultSent();
            var result = new SuccessResult() { Id = Id };
            result.Send(Configuration);
        }

        protected void SendNotFoundResult()
        {
            EnsureAtMostOneResultSent();
            var result = new NotFoundFailedResult() { Id = Id };
            result.Send(Configuration);
        }

        protected void SendTextResult(string text)
        {
            EnsureAtMostOneResultSent();
            var result = new SuccessResult() { Id = Id, ResultText = text};
            result.Send(Configuration);
        }

        protected void SendPositionResult(double left, double top, double width, double height)
        {
            EnsureAtMostOneResultSent();
            var result = new PositionResult()
                             {
                                 Id = Id,
                                 Left = left,
                                 Top = top,
                                 Width = width,
                                 Height = height,
                             };
            result.Send(Configuration);
        }

        protected void SendPictureResult(byte[] bytes)
        {
            var result = new PictureResult()
            {
                Id = Id,
                EncodedPictureBytes = Convert.ToBase64String(bytes),
            };
            result.Send(Configuration);
        }
    }
}