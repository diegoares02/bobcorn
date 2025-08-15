import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import { API_URL } from '../../constants/urls';

export const login = createAsyncThunk(
  'user/login',
  async (credentials: { email: string; password: string }) => {
    const body = {
      Email: credentials.email,
      Password: credentials.password
    }
    const response = await fetch(`${API_URL}/api/user/login`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(body)
    });
    if (!response.ok) {
      throw new Error(`HTTP error! status: ${response.status}`);
    }
    return await response.json();
  }
);

export const register = createAsyncThunk(
  'user/register',
  async (userData: { email: string; password: string; name: string, lastname: string }) => {
    const body = {
      name: userData.name,
      lastname: userData.lastname,
      email: userData.email,
      password: userData.password
    }
    const res = await fetch(`${API_URL}/api/user/register`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(body),
    });
    return await res.json();
  }
);

export const buy = createAsyncThunk(
  'user/buy',
  async (purchaseData: { userId: string }) => {
    console.log(purchaseData.userId)
    const body = {
      productId: 1,
      email: purchaseData.userId
    }
    const token = localStorage.getItem('token');
    const parseToken = token && JSON.parse(token).token;
    const res = await fetch(
      `${API_URL}/api/user/buy`, {
      method: 'POST',
      body: JSON.stringify(body),
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${parseToken}`,
      },
    }
    );
    return await res.json();
  }
);

export const logout = createAsyncThunk(
  'user/logout',
  async () => {
    localStorage.clear();
    return initialState;
  }
);

export interface UserState {
  data: any;
  status: 'idle' | 'loading' | 'succeeded' | 'failed';
  error: string | null;
}

const initialState: UserState = {
  data: null,
  status: 'idle',
  error: null,
};

const userSlice = createSlice({
  name: 'user',
  initialState,
  reducers: {},
  extraReducers: builder => {
    builder
      .addCase(login.fulfilled, (state, action) => {
        state.data = action.payload;
        state.status = 'succeeded';
      })
      .addCase(register.fulfilled, (state, action) => {
        state.data = action.payload;
        state.status = 'succeeded';
      })
      .addCase(buy.fulfilled, (state, action) => {
        // handle buy response if needed
        state.status = 'succeeded';
      })
      .addCase(logout.fulfilled, (state, action) => {
        state.data = action.payload;
        state.status = 'succeeded';
      })
      .addMatcher(
        action => action.type.endsWith('/pending'),
        state => { state.status = 'loading'; }
      )
      .addMatcher(
        action => action.type.endsWith('/rejected'),
        (state, action: { error: { message: string } }) => {
          state.status = 'failed';
          state.error = action.error.message;
        }
      );
  },
});

export default userSlice.reducer;

export const saveTokenOnLogin = (action: any) => {
  if (action.type === login.fulfilled.type) {
    localStorage.setItem('token', JSON.stringify(action.payload.data));
  }
};