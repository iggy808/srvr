namespace Core.Validation.UserService
open System

module GetUserByIdValidation =
  let private UserIdIsNotEmpty userId = userId <> Guid.Empty

  let IsRequestValid request =
    UserIdIsNotEmpty request
