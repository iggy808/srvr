namespace core.repositories

open System
open System.Threading.Tasks
open core.records

type IUserRepository =
  abstract member CreateUser: User -> Task
  abstract member GetUserById: int -> Task<Option<User>>
  abstract member UpdateUser: User -> Task
  abstract member DeleteUserById: int -> Task
