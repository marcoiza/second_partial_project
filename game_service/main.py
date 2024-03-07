from fastapi import FastAPI
from db.routers import user_routes

app = FastAPI()

app.include_router(user_routes.router)

@app.get("/")
async def root():
  return {"Hello": "World"}