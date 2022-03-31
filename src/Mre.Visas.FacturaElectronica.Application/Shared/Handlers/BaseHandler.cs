using AutoMapper;
using Mre.Visas.FacturaElectronica.Application.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mre.Visas.FacturaElectronica.Application.Shared.Handlers
{
  public abstract class BaseHandler
  {
    #region Constructors

    protected BaseHandler()
    {
    }

    protected BaseHandler(IUnitOfWork unitOfWork)
    {
      UnitOfWork = unitOfWork;
    }

    protected BaseHandler(IMapper mapper)
    {
      Mapper = mapper;
    }

    protected BaseHandler(IMapper mapper, IUnitOfWork unitOfWork)
    {
      Mapper = mapper;
      UnitOfWork = unitOfWork;
    }

    #endregion Constructors

    #region Properties

    protected IMapper Mapper;

    protected IUnitOfWork UnitOfWork;

    #endregion Properties
  }
}