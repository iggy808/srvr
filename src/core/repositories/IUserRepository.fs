namespace core.Repositories
open System
open core.Records
open System.Threading.Tasks
type IUserRepository =
  abstract member CreateUser: User -> Task
  abstract member GetUserById: int -> Task<Option<User>>
  abstract member UpdateUser: User -> Task
  abstract member DeleteUserById: int -> Task
