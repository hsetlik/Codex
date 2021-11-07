using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Domain.DataObjects;
using FluentValidation;


namespace Application.DataObjectHandling
{
    public class TermValidator : AbstractValidator<Term>
    {
        public TermValidator()
        {
        }
    }
}