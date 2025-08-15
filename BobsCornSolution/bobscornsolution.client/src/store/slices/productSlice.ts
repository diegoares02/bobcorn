import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import { API_URL } from '../../constants/urls';

export const availableProducts = createAsyncThunk(
  'product/available',
  async () => {
    
    const response = await fetch(`${API_URL}/api/product/available`, {
      method: 'GET'
    });
    if (!response.ok) {
      throw new Error(`HTTP error! status: ${response.status}`);
    }
    return await response.json();
  }
);

export interface ProductState {
  data: any;
  status: 'idle' | 'loading' | 'succeeded' | 'failed';
  error: string | null;
}

const initialState: ProductState = {
  data: null,
  status: 'idle',
  error: null,
};

const productSlice = createSlice({
  name: 'product',
  initialState,
  reducers: {},
  extraReducers: builder => {
    builder
      .addCase(availableProducts.fulfilled, (state, action) => {
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

export default productSlice.reducer;