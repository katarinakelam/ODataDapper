using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ODataDapper.Models
{
    /// <summary>
    /// The racun model class.
    /// </summary>
    public class Racun
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the zaposlenik identifier.
        /// </summary>
        /// <value>
        /// The zaposlenik identifier.
        /// </value>
        public int Zaposlenik_Id { get; set; }

        /// <summary>
        /// Gets or sets the datum izdavanja.
        /// </summary>
        /// <value>
        /// The datum izdavanja.
        /// </value>
        public DateTime DatumIzdavanja { get; set; }

        /// <summary>
        /// Gets or sets the JIR.
        /// </summary>
        /// <value>
        /// The JIR.
        /// </value>
        public string JIR { get; set; }

        /// <summary>
        /// Gets or sets the ukupan iznos.
        /// </summary>
        /// <value>
        /// The ukupan iznos.
        /// </value>
        public float UkupanIznos { get; set; }

        /// <summary>
        /// Gets or sets the stavke.
        /// </summary>
        /// <value>
        /// The stavke.
        /// </value>
        public List<Stavka> Stavke { get; set; }

        /// <summary>
        /// Gets or sets the zaposlenik.
        /// </summary>
        /// <value>
        /// The zaposlenik.
        /// </value>
        public Zaposlenik Zaposlenik { get; set; }
    }
}