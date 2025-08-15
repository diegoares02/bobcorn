import { configureStore } from '@reduxjs/toolkit';
import userReducer, { saveTokenOnLogin } from './slices/userSlice';

const store = configureStore({
    reducer: {
        user: userReducer,
    },
    middleware: getDefaultMiddleware =>
    getDefaultMiddleware().concat(store => next => action => {
      saveTokenOnLogin(action);
      return next(action);
    }),
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;

export default store;