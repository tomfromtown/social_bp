import { useState } from 'react'
import { postsAPI } from '../services/api'
import './CreatePost.css'

function CreatePost({ onPostCreated }) {
  const [content, setContent] = useState('')
  const [loading, setLoading] = useState(false)
  const [error, setError] = useState('')

  const handleSubmit = async (e) => {
    e.preventDefault()
    
    if (!content.trim()) {
      setError('Please enter some content')
      return
    }

    if (content.length > 2000) {
      setError('Post content must be less than 2000 characters')
      return
    }

    setLoading(true)
    setError('')

    try {
      await postsAPI.create(content)
      setContent('')
      // Notify parent to refresh posts
      if (onPostCreated) {
        onPostCreated()
      }
    } catch (err) {
      setError(err.message || 'Failed to create post')
    } finally {
      setLoading(false)
    }
  }

  return (
    <div className="create-post">
      <form onSubmit={handleSubmit} className="create-post-form">
        <textarea
          value={content}
          onChange={(e) => setContent(e.target.value)}
          placeholder="What's on your mind?"
          className="create-post-input"
          rows={3}
          maxLength={2000}
        />
        <div className="create-post-footer">
          <span className="char-count">{content.length}/2000</span>
          {error && <span className="error-text">{error}</span>}
          <button 
            type="submit" 
            className="create-post-button"
            disabled={loading || !content.trim()}
          >
            {loading ? 'Posting...' : 'Post'}
          </button>
        </div>
      </form>
    </div>
  )
}

export default CreatePost

