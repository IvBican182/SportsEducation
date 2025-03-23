namespace Aperta_web_app.Contracts
{
    //"T" represents our data objects
    //acts as a contract
    //Generic repository communicates with our database 
    public interface IGenericRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync(); //returns a list of T (objects) , doesnt take parameters
        Task<T> GetAsync(int? id); //returns a single T object, takes an id
        Task<T> AddAsync(T entity); //returns a single T object, takes an entire object
        Task UpdateAsync(T entity); //dosnt have a return type, takes an object for updating
        Task DeleteAsync(int id); //no return type, takes an id 
        Task<bool> Exists(int id); //does any record with said id exist , returns true or false

    }
}
