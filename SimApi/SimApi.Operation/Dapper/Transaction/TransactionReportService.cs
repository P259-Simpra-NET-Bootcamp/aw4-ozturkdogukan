using AutoMapper;
using Serilog;
using SimApi.Base;
using SimApi.Data;
using SimApi.Data.Uow;
using SimApi.Schema;

namespace SimApi.Operation;

public class TransactionReportService : ITransactionReportService
{

    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public TransactionReportService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    public ApiResponse<List<TransactionViewResponse>> GetAll()
    {
        try
        {
            //var entityList = unitOfWork.DapperTransactionRepository.GetAll();
            var entityList = unitOfWork.Repository<TransactionView>().GetAll();
            var mapped = mapper.Map<List<TransactionView>, List<TransactionViewResponse>>(entityList);
            return new ApiResponse<List<TransactionViewResponse>>(mapped);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "GetAll Exception");
            return new ApiResponse<List<TransactionViewResponse>>(ex.Message);
        }
    }

    public ApiResponse<List<TransactionViewResponse>> GetByAccountId(int accountId)
    {
        try
        {
            //var entityList = unitOfWork.DapperTransactionRepository.GetByAccountId(accountId);
            var entityList = unitOfWork.Repository<TransactionView>().Where(x => x.AccountId.Equals(accountId))?.ToList();
            var mapped = mapper.Map<List<TransactionView>, List<TransactionViewResponse>>(entityList);
            return new ApiResponse<List<TransactionViewResponse>>(mapped);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "GetAll Exception");
            return new ApiResponse<List<TransactionViewResponse>>(ex.Message);
        }
    }

    public ApiResponse<List<TransactionViewResponse>> GetByCustomerId(int customerId)
    {
        try
        {
            //var entityList = unitOfWork.DapperTransactionRepository.GetByCustomerId(customerId);
            var entityList = unitOfWork.Repository<TransactionView>().Where(x => x.CustomerId.Equals(customerId))?.ToList();
            var mapped = mapper.Map<List<TransactionView>, List<TransactionViewResponse>>(entityList);
            return new ApiResponse<List<TransactionViewResponse>>(mapped);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "GetAll Exception");
            return new ApiResponse<List<TransactionViewResponse>>(ex.Message);
        }
    }

    public ApiResponse<TransactionViewResponse> GetById(int id)
    {
        try
        {
            //var entityList = unitOfWork.DapperTransactionRepository.GetById(id);
            var entityList = unitOfWork.Repository<TransactionView>().GetById(id);
            var mapped = mapper.Map<TransactionView, TransactionViewResponse>(entityList);
            return new ApiResponse<TransactionViewResponse>(mapped);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "GetAll Exception");
            return new ApiResponse<TransactionViewResponse>(ex.Message);
        }
    }

    public ApiResponse<List<TransactionViewResponse>> GetByReferenceNumber(string referenceNumber)
    {

        try
        {
            //var entityList = unitOfWork.DapperTransactionRepository.GetByReferenceNumber(referenceNumber);
            var entityList = unitOfWork.Repository<TransactionView>().Where(x => x.ReferenceNumber.Equals(referenceNumber))?.ToList();
            var mapped = mapper.Map<List<TransactionView>, List<TransactionViewResponse>>(entityList);
            return new ApiResponse<List<TransactionViewResponse>>(mapped);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "GetAll Exception");
            return new ApiResponse<List<TransactionViewResponse>>(ex.Message);
        }
    }
}
