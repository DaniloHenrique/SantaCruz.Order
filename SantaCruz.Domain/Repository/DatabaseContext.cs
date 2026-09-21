using ErrorOr;
using System.Data;

namespace SantaCruz.Domain.Repository
{
    public abstract class DatabaseContext(IDbConnection connection) : IDisposable
    {
        public IDbConnection Connection { get; } = connection;

        //Garante que a conexão seja aberta e fechada no mesmo escopo de execução
        public async Task<ErrorOr<T>> Execute<T>(Func<Task<ErrorOr<T>>> action)
        {
            Connection.Open();

            try
            {
                return await action();
            }
            catch (Exception ex)
            {
                return Error.Failure(description: ex.Message, code: "DatabaseOperationError");
            }
            finally
            {
                Connection.Close();
            }
        }

        //Cria um escopo de transação para garantir integridade na base e executa a operação dentro desse mesmo escopo.
        public async Task<ErrorOr<T>> TransactionedOperation<T>(Func<Task<ErrorOr<T>>> action)
        {
            Connection.Open();

            using var transaction = Connection.BeginTransaction();

            try
            {
                var result = await action();

                if (result.IsError)
                {
                    transaction.Rollback();
                    return result;
                }

                transaction.Commit();

                return result;
            }
            catch (Exception ex)
            {
                transaction.Rollback();

                return Error.Failure(description: ex.Message,code: "DatabaseOperationError");
            }
            finally
            {
                Connection.Close();
            }
        }

        public void Dispose()
        {
            if (Connection.State != ConnectionState.Closed) Connection.Close();
            Connection.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
