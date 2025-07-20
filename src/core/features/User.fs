namespace core.features

module UserF =
    type Features = {
        GetUser : int -> Task<Option<User>>,
        CreateUser : User -> Task,
        UpdateUser : User -> Task,
        DeleteUser : User -> Task
    }