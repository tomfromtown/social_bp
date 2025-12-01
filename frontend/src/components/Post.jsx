import { useState } from 'react'
import CommentSection from './CommentSection'
import './Post.css'

function Post({ post, currentUser, onLike, onAddComment }) {
  const isLiked = post.isLiked !== undefined ? post.isLiked : post.likedBy.includes(currentUser)

  const handleLikeClick = () => {
    onLike(post.id)
  }

  const handleAddComment = (commentText) => {
    onAddComment(post.id, commentText)
  }

  return (
    <div className="post">
      <div className="post-header">
        <div className="post-author">
          <div className="author-avatar">
            {post.author.charAt(0).toUpperCase()}
          </div>
          <div className="author-info">
            <span className="author-name">{post.author}</span>
            <span className="post-timestamp">{post.timestamp}</span>
          </div>
        </div>
      </div>

      <div className="post-content">
        <p>{post.content}</p>
      </div>

      <div className="post-actions">
        <button 
          className={`like-button ${isLiked ? 'liked' : ''}`}
          onClick={handleLikeClick}
          aria-label="Like post"
        >
          <span className="like-icon">{isLiked ? '❤️' : '🤍'}</span>
          <span className="like-count">{post.likes}</span>
        </button>
      </div>

      <CommentSection 
        comments={post.comments}
        currentUser={currentUser}
        onAddComment={handleAddComment}
      />
    </div>
  )
}

export default Post

