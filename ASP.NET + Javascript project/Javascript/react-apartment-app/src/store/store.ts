import { configureStore } from '@reduxjs/toolkit'
import identityReducer from './identity'
import associationReducer from './associations'

export const store = configureStore({
    reducer: {
      identity: identityReducer,
      association: associationReducer,
    }
  })



// Infer the `RootState` and `AppDispatch` types from the store itself
export type RootState = ReturnType<typeof store.getState>
// Inferred type: {posts: PostsState, comments: CommentsState, users: UsersState}
export type AppDispatch = typeof store.dispatch