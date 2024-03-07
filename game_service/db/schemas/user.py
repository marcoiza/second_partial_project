def user_schema(user) -> dict:
    return {"username": user["username"],
            "score": user["score"]}


def users_schema(users) -> list:
    return [user_schema(user) for user in users]