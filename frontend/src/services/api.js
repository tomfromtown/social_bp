const API_BASE_URL = import.meta.env.VITE_API_URL || 'http://localhost:5000/api'

// Get token from localStorage
export const getToken = () => {
  return localStorage.getItem('token')
}

// Set token in localStorage
export const setToken = (token) => {
  localStorage.setItem('token', token)
}

// Remove token from localStorage
export const removeToken = () => {
  localStorage.removeItem('token')
}

// Create fetch request with authentication
const apiRequest = async (url, options = {}) => {
  const token = getToken()
  
  const headers = {
    'Content-Type': 'application/json',
    'Cache-Control': 'no-cache, no-store, must-revalidate',
    'Pragma': 'no-cache',
    'Expires': '0',
    ...options.headers,
  }

  if (token) {
    headers['Authorization'] = `Bearer ${token}`
  }

  const config = {
    ...options,
    headers,
    cache: 'no-store', // Prevent browser caching
  }

  try {
    const response = await fetch(`${API_BASE_URL}${url}`, config)
    
    // Handle 304 Not Modified - force a fresh request
    if (response.status === 304) {
      // Retry with cache-busting timestamp
      const cacheBustUrl = `${API_BASE_URL}${url}${url.includes('?') ? '&' : '?'}_t=${Date.now()}`
      const retryResponse = await fetch(cacheBustUrl, { ...config, cache: 'no-store' })
      if (!retryResponse.ok && retryResponse.status !== 304) {
        const errorData = await retryResponse.json().catch(() => ({}))
        throw new Error(errorData.message || `HTTP error! status: ${retryResponse.status}`)
      }
      return await retryResponse.json()
    }
    
    if (!response.ok) {
      if (response.status === 401) {
        // Unauthorized - remove token and redirect to login
        removeToken()
        throw new Error('Unauthorized')
      }
      const errorData = await response.json().catch(() => ({}))
      throw new Error(errorData.message || `HTTP error! status: ${response.status}`)
    }

    return await response.json()
  } catch (error) {
    console.error('API request failed:', error)
    throw error
  }
}

// Auth API
export const authAPI = {
  login: async (username, password) => {
    return apiRequest('/auth/login', {
      method: 'POST',
      body: JSON.stringify({ username, password }),
    })
  },
}

// Posts API
export const postsAPI = {
  getAll: async () => {
    return apiRequest('/posts')
  },
  create: async (content) => {
    return apiRequest('/posts', {
      method: 'POST',
      body: JSON.stringify({ content }),
    })
  },
}

// Comments API
export const commentsAPI = {
  add: async (postId, content) => {
    return apiRequest(`/posts/${postId}/comments`, {
      method: 'POST',
      body: JSON.stringify({ content }),
    })
  },
}

