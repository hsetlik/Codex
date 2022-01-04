using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.DataObjects
{
    public class CollectionMember
    {
        [Key]
        public Guid CollectionMemberId { get; set; }
        //Nav properties / foreign keys
        public Guid CollectionId { get; set; }
        public Collection Collection { get; set; }

        public Guid ContentId { get; set; }
        public Content Content { get; set; }

    }
}