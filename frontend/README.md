# Social Media Frontend

A React-based social media application with login, posts, comments, and likes functionality.

## Features

- **Login**: Simple username/password authentication
- **Post Feed**: View posts from different users
- **Likes**: Like/unlike posts with visual feedback
- **Comments**: Add comments to posts with a text input

## Getting Started

### Prerequisites

- Node.js (v16 or higher)
- npm or yarn

### Installation

1. Navigate to the frontend directory:
```bash
cd frontend
```

2. Install dependencies:
```bash
npm install
```

### Running the Application

Start the development server:
```bash
npm run dev
```

The application will be available at `http://localhost:5173` (or the port shown in the terminal).

### Building for Production

To create a production build:
```bash
npm run build
```

To preview the production build:
```bash
npm run preview
```

## Project Structure

```
frontend/
├── src/
│   ├── components/
│   │   ├── Login.jsx           # Login component
│   │   ├── PostList.jsx        # List of posts
│   │   ├── Post.jsx            # Individual post component
│   │   ├── CreatePost.jsx      # Post creation form
│   │   └── CommentSection.jsx  # Comments section
│   ├── services/
│   │   └── api.js             # API service for backend communication
│   ├── utils/
│   │   └── formatDate.js      # Date formatting utility
│   ├── App.jsx                 # Main app component
│   ├── main.jsx                # Entry point
│   └── index.css               # Global styles
├── index.html
├── package.json
└── vite.config.js
```

## API Configuration

The frontend connects to the backend API. By default, it uses `http://localhost:5000/api`.

To change the API URL, create a `.env` file in the frontend directory:

```
VITE_API_URL=http://localhost:5000/api
```

## Usage

1. **Login**: Use the test credentials (username: `test`, password: `test`) or any valid credentials
2. **View Posts**: After logging in, you'll see a feed of posts from the backend
3. **Create Posts**: Use the post creation form at the top of the feed
4. **Like Posts**: Click the heart icon to like/unlike a post (client-side only, backend endpoint not implemented)
5. **Add Comments**: Type in the comment box below any post and click "Post" to add a comment

## Features

- **JWT Authentication**: Tokens are stored in localStorage
- **Automatic Token Management**: Tokens are automatically included in API requests
- **Real-time Updates**: Posts and comments are fetched from the backend API
- **Error Handling**: User-friendly error messages for API failures
- **Loading States**: Visual feedback during API operations

## Technologies Used

- React 18
- Vite
- CSS3 (Modern styling with gradients and animations)

