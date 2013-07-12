using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ContactsManager.Web.Api.Models;

using System.Web.Http;

namespace ContactsManager.Web.Api.Messages
{
    public class ContactRequest
    {
        public ContactBO contact { get; set; }
    }
}