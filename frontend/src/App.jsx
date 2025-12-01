import { useState, useEffect } from 'react'
import Login from './components/Login'
import PostList from './components/PostList'
import { getToken, removeToken } from './services/api'
import './App.css'

function App() {
  const [isLoggedIn, setIsLoggedIn] = useState(false)
  const [currentUser, setCurrentUser] = useState(null)
  const [isInitialized, setIsInitialized] = useState(false)

  // Check if user is already logged in (has token)
  useEffect(() => {
    try {
      const token = getToken()
      if (token) {
        // Token exists, but we need username - try to get from localStorage
        const username = localStorage.getItem('username')
        if (username) {
          setCurrentUser(username)
          setIsLoggedIn(true)
        }
      }
    } catch (error) {
      console.error('Error checking login status:', error)
    } finally {
      setIsInitialized(true)
    }
  }, [])

  const handleLogin = (username) => {
    setCurrentUser(username)
    setIsLoggedIn(true)
    // Store username for persistence
    localStorage.setItem('username', username)
  }

  const handleLogout = () => {
    setCurrentUser(null)
    setIsLoggedIn(false)
    removeToken()
    localStorage.removeItem('username')
  }

  if (!isInitialized) {
    return (
      <div className="app" style={{ 
        display: 'flex', 
        justifyContent: 'center', 
        alignItems: 'center', 
        minHeight: '100vh',
        color: 'white',
        fontSize: '18px'
      }}>
        Loading...
      </div>
    )
  }

  return (
    <div className="app">
      {!isLoggedIn ? (
        <Login onLogin={handleLogin} />
      ) : (
        <div className="app-container">
          <header className="app-header">
            <h1>The Social Media Site</h1>
            <div className="user-info">
              <span>Welcome, {currentUser}!</span>
              <button onClick={handleLogout} className="logout-btn">
                Logout
              </button>
            </div>
          </header>
          <PostList currentUser={currentUser} />
        </div>
      )}
    </div>
  )
}

export default App

