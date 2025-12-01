import { useState } from 'react'
import './CommentSection.css'

function CommentSection({ comments, currentUser, onAddComment }) {
  const [commentText, setCommentText] = useState('')
  const [isExpanded, setIsExpanded] = useState(false)

  const handleSubmit = (e) => {
    e.preventDefault()
    if (commentText.trim()) {
      onAddComment(commentText)
      setCommentText('')
    }
  }

  return (
    <div className="comment-section">
      {comments.length > 0 && (
        <div className="comments-list">
          {comments.map(comment => (
            <div key={comment.id} className="comment">
              <div className="comment-author-avatar">
                {comment.author.charAt(0).toUpperCase()}
              </div>
              <div className="comment-content-wrapper">
                <div className="comment-header">
                  <span className="comment-author">{comment.author}</span>
                  <span className="comment-timestamp">{comment.timestamp}</span>
                </div>
                <p className="comment-text">{comment.content}</p>
              </div>
            </div>
          ))}
        </div>
      )}

      <form onSubmit={handleSubmit} className="comment-form">
        <div className="comment-input-wrapper">
          <div className="comment-input-avatar">
            {currentUser.charAt(0).toUpperCase()}
          </div>
          <input
            type="text"
            value={commentText}
            onChange={(e) => setCommentText(e.target.value)}
            placeholder="Write a comment..."
            className="comment-input"
          />
          <button 
            type="submit" 
            className="comment-submit-btn"
            disabled={!commentText.trim()}
          >
            Post
          </button>
        </div>
      </form>
    </div>
  )
}

export default CommentSection

