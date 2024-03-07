from fastapi import APIRouter, HTTPException, status, Depends
from db.models.user import User
from db.schemas.user import user_schema, users_schema
from db.client import db_client
from bson import ObjectId
from typing import Annotated
from typing import List

router = APIRouter(prefix="/users",
                  tags=["users"],
                  responses={status.HTTP_404_NOT_FOUND: {"message": "No encontrado"}})


@router.get("/", response_model=list[User])
async def users():
  return users_schema(db_client.gameScores.find())
  

@router.post("/", response_model=User, status_code=status.HTTP_201_CREATED)
async def user(user: User):
  user_dict = dict(user)
  db_client.gameScores.insert_one(user_dict)

  new_user = user_schema(db_client.gameScores.find_one({"username": user.username}))

  return User(**new_user)

@router.get("/list", response_model=List[User])
async def get_users():
  users = []
  for user_dict in db_client.gameScores.find():
    users.append(user_schema(user_dict))
  return users