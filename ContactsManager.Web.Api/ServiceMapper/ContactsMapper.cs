using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using AutoMapper;
using ContactsManager.Web.Api.Models;
using Contacts.Data.SqlServer.Entities;

namespace ContactsManager.Web.Api.ServiceMapper
{
       public class ContactsMap
        {
            public static ContactBO Map(Contact entity)
            {
                return Mapper.Map<Contact, ContactBO>(entity);
            }

            public static List<ContactBO> Map(List<Contact> entities)
            {
                return Mapper.Map<List<Contact>, List<ContactBO>>(entities);
            }

           public static Contact Map(ContactBO entityBO)
            {
                return Mapper.Map<ContactBO, Contact>(entityBO);
            }
        }
    
}