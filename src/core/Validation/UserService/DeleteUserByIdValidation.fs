namespace Core.Validation.UserService
open System

module DeleteUserByIdValidation =
  let private UserIdIsNotEmpty userId = userId <> Guid.Empty

  let IsRequestValid request =
   UserIdIsNotEmpty request 
