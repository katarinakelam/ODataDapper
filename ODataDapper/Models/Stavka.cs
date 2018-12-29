using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ODataDapper.Models
{
    /// <summary>
    /// The stavka model class.
    /// </summary>
    public class Stavka
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the naziv.
        /// </summary>
        /// <value>
        /// The naziv.
        /// </value>
        public string Naziv { get; set; }

        /// <summary>
        /// Gets or sets the opis.
        /// </summary>
        /// <value>
        /// The opis.
        /// </value>
        public string Opis { get; set; }

        /// <summary>
        /// Gets or sets the cijena.
        /// </summary>
        /// <value>
        /// The cijena.
        /// </value>
        public float Cijena { get; set; }
    }
}