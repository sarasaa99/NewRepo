using Microsoft.AspNetCore.Identity;
using Shatably.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shatably.BLL.Model
{
    public class OperationResult
    {
        protected OperationResult()
        {
            this.Success = true;
        }
        protected OperationResult(ApplicationUser User, IdentityResult identityResult)
        {
            this.Success = true;
            this.IdentityResult = identityResult;
            this.applicationUser = User;
        }
        protected OperationResult(IdentityResult identityResult)
        {
            this.Success = false;
            this.IdentityResult = identityResult;
        }
        protected OperationResult(string message)
        {
            this.Success = false;
            this.FailureMessage = message;
        }
        protected OperationResult(Exception ex)
        {
            this.Success = false;
            this.Exception = ex;
        }
        public bool Success { get; protected set; }
        public string FailureMessage { get; protected set; }
        public Exception Exception { get; protected set; }
        public ApplicationUser applicationUser { get; protected set; }
        public IdentityResult IdentityResult { get; protected set; }

        public static OperationResult SuccessResult()
        {
            return new OperationResult();
        }
        public static OperationResult SuccessResultAdd(ApplicationUser applicationUser, IdentityResult identityResult)
        {
            return new OperationResult(applicationUser, identityResult);
        }
        public static OperationResult FailureResult(string message)
        {
            return new OperationResult(message);
        }
        public static OperationResult FailureResult(IdentityResult identityResult)
        {
            return new OperationResult(identityResult);
        }
        public static OperationResult ExceptionResult(Exception ex)
        {
            return new OperationResult(ex);
        }
        public bool IsException()
        {
            return this.Exception != null;
        }
    }
}
