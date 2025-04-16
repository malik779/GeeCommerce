using System.Net;
using System.Xml.Serialization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Gee.Core.Responses
{
    public partial record Response<TEntity> where TEntity : class
    {
       
        public Response(TEntity? data, HttpStatusCode status =HttpStatusCode.OK,string? message =null)
        {
            this.Data = data;
            StatusCode = status;
            Message = message;
            CustomProperties = new Dictionary<string, string>();
            PostInitialize();
        }


        #region Methods

        /// <summary>
        /// Perform additional actions for the model initialization
        /// </summary>
        /// <remarks>Developers can override this method in custom partial classes in order to add some custom initialization code to constructors</remarks>
        protected virtual void PostInitialize()
        {
        }

        #endregion

        /// <summary>
        /// Gets or sets property to store any custom values for models 
        /// </summary>
        [XmlIgnore]
        public Dictionary<string, string> CustomProperties { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string? Message { get; set; }
        public TEntity? Data { get; set; }
};
    
}
