from flask import Flask, render_template, request
from flask_socketio import SocketIO

app = Flask(__name__)
socketio = SocketIO(app)

@app.route('/')
def index():
    return render_template('index.html')

@socketio.on('message_from_client')
def handle_message(message):
    print('Message from client:', message)

@app.route('/send_message', methods=['POST'])
def send_message():
    message = request.form.get('message')
    print('Message received from Unity:', message)
    return 'Message received successfully!'

if __name__ == '__main__':
    socketio.run(app, host='0.0.0.0', port=5000, debug=True)