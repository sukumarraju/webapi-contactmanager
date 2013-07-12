using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Contacts.Data.SqlServer.Repository;
using AutoMapper;

using ContactsManager.Web.Api.Models;
using ContactsManager.Web.Api.Messages;
using Contacts.Data.SqlServer.Entities;
using ContactsManager.Web.Api.ServiceMapper;

namespace ContactsManager.Web.Api.Controllers
{
    public class ContactsController : ApiController
    {
        /// <summary>
        /// Initialise a variable of IContactsManagerRepository from data layer
        /// </summary>
        private readonly IContactsManagerRepository Contactrepository;

        /// <summary>
        /// Inject repository
        /// </summary>
        /// <param name="_repository">IContactsManagerRepository</param>
        public ContactsController(IContactsManagerRepository _repository)
        {
            if(_repository == null)
            {
                throw new ArgumentNullException("ContactManager Repository exception");
            }

            this.Contactrepository = _repository;
            
            //Map POCO <----> BO
            Mapper.CreateMap<Contact, ContactBO>();
            Mapper.CreateMap<ContactBO, Contact>();

            Mapper.AssertConfigurationIsValid();
        }

        /// <summary>
        /// Return list of contact records
        /// </summary>
        /// <returns>list of contact DTO</returns>
        public ContactResponse Get()
        {
            ContactResponse response = new ContactResponse();

            try
            {
                response.contacts = Mapper.Map<List<Contact>, List<ContactBO>>(this.Contactrepository.GetAllContacts());
            }
            catch (Exception e)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent("An error occurred, please try again"),
                    ReasonPhrase = "Critical exception"
                });
            }
            
            return response;
        }

        /// <summary>
        /// Returns a single contact record that matches with the id 
        /// </summary>
        /// <param name="id">contact id</param>
        /// <returns>single contact DTO</returns>
        public ContactResponse Get(int id)
        {
            ContactResponse response = new ContactResponse();
            response.contact = Mapper.Map<Contact, ContactBO>(this.Contactrepository.GetContactById(id));
            
            if(response.contact == null)
            {
                var resp = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent(string.Format("No Contact with ID = {0}", id)),
                    ReasonPhrase = "Contact ID Not Found"
                };
                
                throw new HttpResponseException(resp);
            }

            return response;
        }

        [HttpPost]
        public HttpResponseMessage Post(ContactBO  request)
        {
            var data = ContactsMap.Map(request);

            var entity = Contactrepository.Insert(data);
            var response = Request.CreateResponse(HttpStatusCode.Created, entity);
            
            try
            {
            string uri = Url.Link("DefaultApi", new { id = entity.ID });
                response.Headers.Location = new Uri(uri);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent("An error occurred, please try again"),
                    ReasonPhrase = "Critical exception"
                });
            }
            
            return response;
        }

        public HttpResponseMessage Put(int id, ContactBO contact)
        {
            contact.Id = id;
            var entity = ContactsMap.Map(contact);

            if(!Contactrepository.Update(entity))
            {
                throw new HttpResponseException(HttpStatusCode.NotModified);
            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            
        }

        public void Delete(int id)
        {
            ContactBO contact = ContactsMap.Map(this.Contactrepository.GetContactById(id));

            if(contact == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            Contactrepository.Delete(id);
        }
    }

    
}
