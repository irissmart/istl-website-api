using Framework.Model;
using System.Collections;

namespace Framework.Service
{
    public class BaseService
    {
        private bool _success;
        private string _code;
        private string _message;

        public void InitMessageResponse(string messageResponseKey = "Success", string? customMessage = null)
        {
            var result = MessageResponse.Responses[messageResponseKey];

            _success = result.Item1;
            _code = result.Item2;
            _message = customMessage == null ? result.Item3 : customMessage;
        }

        protected async Task<BaseResponse<TResult>> HandleActionAsync<TResult>(Func<Task<TResult>> action)
        {
            TResult result = default;
            try
            {
                InitMessageResponse();
                result = await action();
                if ((result == null || result is ICollection && (result as ICollection).Count == 0) && _code == "200")
                {
                    InitMessageResponse("NotFound");
                }
            }
            catch (Exception ex)
            {
                InitMessageResponse("ServerError");
            }

            return new BaseResponse<TResult>(_success, _code, _message, result);
        }

        protected async Task<BaseResponse<dynamic>> HandleActionAsync(Func<Task<dynamic>> action)
        {
            dynamic result = default;
            try
            {
                InitMessageResponse();
                result = await action();
                if ((result == null || result is ICollection && (result as ICollection).Count == 0) && _code == "200")
                {
                    InitMessageResponse("NotFound");
                }
            }
            catch (Exception ex)
            {
                InitMessageResponse("ServerError");
            }

            return new BaseResponse<dynamic>(_success, _code, _message, result);
        }

        protected async Task<BaseResponse<Task>> HandleVoidActionAsync(Func<Task> action)
        {
            try
            {
                InitMessageResponse();
                await action();
            }
            catch (Exception ex)
            {
                InitMessageResponse("ServerError");
            }

            return new BaseResponse<Task>(_success, _code, _message, default);
        }

        protected async Task<PagedBaseResponse<List<TResult>>> HandlePaginatedActionAsync<TEntity, TResult>(
            Func<Task<PaginatedResult<TEntity>>> action,
            Func<TEntity, TResult>? mapper = null)
        {
            PaginatedResult<TEntity> paginatedResult = default;
            try
            {
                InitMessageResponse();
                paginatedResult = await action();
                if (paginatedResult == null || paginatedResult.Items == null || paginatedResult.Items.Count == 0)
                {
                    InitMessageResponse("NotFound");
                }
            }
            catch (Exception ex)
            {
                InitMessageResponse("ServerError");
            }

            List<TResult> mappedItems;
            if (mapper != null)
            {
                mappedItems = paginatedResult.Items?.Select(mapper).ToList();
            }
            else
            {
                // Directly cast the items to TResult if no mapper is provided
                mappedItems = paginatedResult.Items?.Cast<TResult>().ToList();
            }

            int totalPages = (int)Math.Ceiling((double)paginatedResult.TotalItems / paginatedResult.PageSize);

            return new PagedBaseResponse<List<TResult>>(
                _success,
                _code,
                _message,
                mappedItems,
                paginatedResult.PageNumber,
                paginatedResult.PageSize,
                paginatedResult.TotalItems,
                totalPages
            );
        }

        protected async Task<PagedBaseResponse<List<dynamic>>> HandlePaginatedActionAsync<TEntity>(
            Func<Task<PaginatedResult<TEntity>>> action,
            Func<TEntity, dynamic>? mapper = null)
        {
            PaginatedResult<TEntity> paginatedResult = default;
            try
            {
                InitMessageResponse();
                paginatedResult = await action();
                if (paginatedResult == null || paginatedResult.Items == null || paginatedResult.Items.Count == 0)
                {
                    InitMessageResponse("NotFound");
                }
            }
            catch (Exception ex)
            {
                InitMessageResponse("ServerError");
            }

            List<dynamic> mappedItems;
            if (mapper != null)
            {
                mappedItems = paginatedResult.Items?.Select(mapper).ToList();
            }
            else
            {
                mappedItems = paginatedResult.Items?.Cast<dynamic>().ToList();
            }

            int totalPages = 0;
            if (paginatedResult.PageSize != 0)
            {
                totalPages = (int)Math.Ceiling((double)paginatedResult.TotalItems / paginatedResult.PageSize);
            }

            return new PagedBaseResponse<List<dynamic>>(
                _success,
                _code,
                _message,
                mappedItems,
                paginatedResult.PageNumber,
                paginatedResult.PageSize,
                paginatedResult.TotalItems,
                totalPages
            );
        }
    }
}