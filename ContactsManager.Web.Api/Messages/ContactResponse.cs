using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ContactsManager.Web.Api.Models;

namespace ContactsManager.Web.Api.Messages
{
    public class ContactResponse
    {
        public List<ContactBO> contacts;

        public ContactBO contact;

    }
}