using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using UserRegistration.Application.Interfaces;
using UserRegistration.Application.Services.LoanApplicationService.DTOs;
using UserRegistration.Application.Wrapper;
using UserRegistration.Domain;
using UserRegistration.Enums;

namespace UserRegistration.Application.Services.LoanApplicationService
{
 
    public class LoanApplicationService : ILoanApplicationService
    {

        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;

        public LoanApplicationService(IRepositoryAsync repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }


        //Get 
        public async Task<PaginatedResponse<LoanApplicationDTO>> GetAllLoanApplicationsAsync(LoanApplicationListFilter filter)
        {

            //search keyword expression
            Expression<Func<LoanApplicationEntity, bool>> searchKeywordExpression = null;
            if (!String.IsNullOrWhiteSpace(filter.Keyword))
            {
                searchKeywordExpression = (x => x.ApplicationName.ToLower().Contains(filter.Keyword.ToLower()));

            }

            //paginated response from repository, entity mapped to dto
            var pagedResponse = await _repository.GetPaginatedResultsAsync<LoanApplicationEntity, LoanApplicationDTO>(filter.PageNumber, filter.PageSize, searchKeywordExpression);


            return pagedResponse;

        }

        //Get by Id
        public async Task<IResponse<LoanApplicationDTO>> GetLoanApplication(Guid id)
        {
            try
            {
                var loanApplicationDto = await _repository.GetByIdAsync<LoanApplicationEntity, LoanApplicationDTO>(id);

                return Response<LoanApplicationDTO>.Success(loanApplicationDto);
            }
            catch (Exception ex)
            {
                return Response<LoanApplicationDTO>.Fail(ex.Message);
            }
        }


        //Create
        public async Task<IResponse<Guid>> CreateLoanApplicationAsync(CreateLoanApplicationRequest request)
        {

            bool isLoanApplicationExists = await _repository.ExistsAsync<LoanApplicationEntity>(x => x.ApplicationName == request.ApplicationName);

            if (isLoanApplicationExists)
                return Response<Guid>.Fail("Loan application exists");

            //Mapping the values
            LoanApplicationEntity loanapplication = new LoanApplicationEntity();
            loanapplication = _mapper.Map(request, loanapplication);


            try
            {
                var loanApplicationId = await _repository.CreateAsync(loanapplication);
                await _repository.SaveChangesAsync();

                return Response<Guid>.Success(loanApplicationId);
            }
            catch (Exception ex)
            {
                return Response<Guid>.Fail(ex.Message);
            }
        }


        //Update
        public async Task<IResponse<Guid>> UpdateLoanApplicationAsync(UpdateLoanApplicationRequest request, Guid id)
        {
            //Get by Id
            var loanapplication = await _repository.GetByIdAsync<LoanApplicationEntity>(id);

            //Check if exists
            if (loanapplication == null)
                return Response<Guid>.Fail("Not Found");

            if(loanapplication.Status != StatusEnum.Rejected)
                return Response<Guid>.Fail("You can only edit application when it is rejected");

            //Mapping
            var updatedloanapplication = _mapper.Map(request, loanapplication);


            try
            {
                await _repository.UpdateAsync(updatedloanapplication);
                await _repository.SaveChangesAsync();

                return Response<Guid>.Success(id);

            }
            catch (Exception ex)
            {
                return Response<Guid>.Fail(ex.Message);
            }
        }



        
    }


}












