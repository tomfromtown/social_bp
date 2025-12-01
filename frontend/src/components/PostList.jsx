import { useState, useEffect } from 'react'
import Post from './Post'
import CreatePost from './CreatePost'
import { postsAPI, commentsAPI } from '../services/api'
import { formatDate } from '../utils/formatDate'
import './PostList.css'

function PostList({ currentUser }) {
  const [posts, setPosts] = useState([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState('')

  useEffect(() => {
    loadPosts()
  }, [])

  const loadPosts = async () => {
    try {
      setLoading(true)
      setError('')
      const data = await postsAPI.getAll()
      
      // Ensure data is an array
      if (!Array.isArray(data)) {
        console.error('Invalid data format:', data)
        setError('Invalid response from server')
        setPosts([])
        return
      }
      
      // Transform API response to match component format
      const transformedPosts = data.map(post => ({
        id: post.id,
        author: post.author || 'Unknown',
        content: post.content || '',
        timestamp: post.createdAt ? formatDate(post.createdAt) : 'Unknown time',
        likes: post.likeCount || 0,
        likedBy: [], // Backend doesn't track who liked, just count
        comments: Array.isArray(post.comments) ? post.comments.map(comment => ({
          id: comment.id,
          author: comment.author || 'Unknown',
          content: comment.content || '',
          timestamp: comment.createdAt ? formatDate(comment.createdAt) : 'Unknown time'
        })) : []
      }))
      
      setPosts(transformedPosts)
    } catch (err) {
      setError(err.message || 'Failed to load posts')
      console.error('Error loading posts:', err)
      setPosts([]) // Set empty array on error
    } finally {
      setLoading(false)
    }
  }

  const handleLike = (postId) => {
    // Note: Backend doesn't have like endpoint yet, so this is client-side only
    // In a real implementation, you'd call an API endpoint here
    setPosts(posts.map(post => {
      if (post.id === postId) {
        const isLiked = post.likedBy.includes(currentUser)
        return {
          ...post,
          likes: isLiked ? post.likes - 1 : post.likes + 1,
          likedBy: isLiked 
            ? post.likedBy.filter(user => user !== currentUser)
            : [...post.likedBy, currentUser]
        }
      }
      return post
    }))
  }

  const handleAddComment = async (postId, commentText) => {
    if (!commentText.trim()) return

    try {
      const newComment = await commentsAPI.add(postId, commentText)
      
      // Update posts with new comment
      setPosts(posts.map(post => {
        if (post.id === postId) {
          return {
            ...post,
            comments: [
              ...post.comments,
              {
                id: newComment.id,
                author: newComment.author,
                content: newComment.content,
                timestamp: formatDate(newComment.createdAt)
              }
            ]
          }
        }
        return post
      }))
    } catch (err) {
      console.error('Error adding comment:', err)
      alert('Failed to add comment: ' + (err.message || 'Unknown error'))
    }
  }

  if (loading) {
    return (
      <div className="post-list">
        <div className="post-list-header">
          <h2>Feed</h2>
        </div>
        <div className="loading-message">Loading posts...</div>
      </div>
    )
  }

  if (error) {
    return (
      <div className="post-list">
        <div className="post-list-header">
          <h2>Feed</h2>
        </div>
        <div className="error-message">{error}</div>
        <button onClick={loadPosts} className="retry-button">Retry</button>
      </div>
    )
  }

  return (
    <div className="post-list">
      <div className="post-list-header">
        <h2>Feed</h2>
      </div>
      <CreatePost onPostCreated={loadPosts} />
      <div className="posts-container">
        {posts.length === 0 ? (
          <div className="no-posts">No posts yet. Be the first to post!</div>
        ) : (
          posts.map(post => (
            <Post
              key={post.id}
              post={post}
              currentUser={currentUser}
              onLike={handleLike}
              onAddComment={handleAddComment}
            />
          ))
        )}
      </div>
    </div>
  )
}

export default PostList

