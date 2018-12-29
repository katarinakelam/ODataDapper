using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ODataDapper.Models
{
    /// <summary>
    /// The Zaposlenik model class.
    /// </summary>
    public class Zaposlenik
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the IME.
        /// </summary>
        /// <value>
        /// The IME.
        /// </value>
        public string Ime { get; set; }

        /// <summary>
        /// Gets or sets the prezime.
        /// </summary>
        /// <value>
        /// The prezime.
        /// </value>
        public string Prezime { get; set; }

        /// <summary>
        /// Gets or sets the datum rodjenja.
        /// </summary>
        /// <value>
        /// The datum rodjenja.
        /// </value>
        public DateTime DatumRodjenja { get; set; }

        /// <summary>
        /// Gets or sets the adresa.
        /// </summary>
        /// <value>
        /// The adresa.
        /// </value>
        public string Adresa { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Zaposlenik"/> has dopustenje.
        /// </summary>
        /// <value>
        ///   <c>true</c> if has dopustenje; otherwise, <c>false</c>.
        /// </value>
        public bool Dopustenje { get; set; }
    }
}