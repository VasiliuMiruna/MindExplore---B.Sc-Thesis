<template>
  <div class="page">
    <PageTitleBar title="Conversații"/>
    <div class="page-wrapper">
      <v-card class="name-list" >
        <v-list>
          <v-list-item v-for="conversation in conversations"
                       :key="conversation.conversationId"
                       :value="conversation.conversationId"
                       @click="navigateToConversation(conversation.conversationId)">
            <v-list-item-content variant="outlined">
              <v-list-item-title >
                {{ conversation.therapist.firstName }} 
                {{
                  conversation.therapist.lastName
                }}
              </v-list-item-title>
            </v-list-item-content>
          </v-list-item>

        </v-list>
      </v-card>
      <v-card class="message-chat" style="max-height: 100%; overflow-y: hidden; display:flex; flex-direction: column; ">
        <div style="overflow-y: scroll; height:0; flex-grow: 1;" ref="messageContainer">
          <div v-for="(message, index) in messages" :key="index" :class="getMessageContainerClass(message.sender)">
            <div class="message-content">
              <template v-if="message.text">
                <p class="text">{{ message.text }}</p>
              </template>
            </div>
          </div>
          <div style="height: 50px;" id="scroll-target"></div>
        </div>
        <div class="footer-chat">
          <v-form @submit.prevent="sendMessage" class="input-container">
            <input type="text" placeholder="Type your message here" v-model="messageText"/>
            <v-btn type="submit" color="primary">Send</v-btn>
          </v-form>

        </div>
      </v-card>
      <v-dialog v-model="imageModalVisible" max-width="500">
        <v-img :src="selectedImage" :alt="selectedImageAlt" contain max-height="400"></v-img>
      </v-dialog>

    </div>
  </div>
</template>

<script>
import PageTitleBar from '@/components/PageTitleBar.vue';
import * as signalR from "@microsoft/signalr";
import axios from 'axios';
import {API_BASE} from '@/services/constants';
import {mapState} from "pinia";
import {useUserStore} from "@/store/user";

export default {
  components: {
    PageTitleBar,
  },
  props: {
    sender: {
      type: String,
      required: true,
    },
    receiver: {
      type: String,
      required: true,
    },
  },
  data() {
    return {
      file: '',
      messageText: "",
      messages: [],
      connection: null,
      receiverUserId: "",
      conversations: [],
      therapists: [],
      currentConversation: this.$route.params.id
    };
  },
  mounted() {
    this.initializeChat();
    this.getMessages(this.currentConversation);
  },
  methods: {
    async initializeChat() {
      await this.getConversations();
      await this.fetchReceiverUserId();
      this.startConnection();
    },
    getWSRoomName() {
      //this.selectedConversationId = this.conversations[0].conversationId;
      //return this.conversations[0].conversationId;
      return this.currentConversation;
    },
    startConnection() {
      this.connection = new signalR.HubConnectionBuilder().withUrl(`${API_BASE}/chatHub`).build();

      this.connection
          .start()
          .then(() => {
            this.getMessages();
          })
          .catch((error) => {
            console.error(error);
          });

      this.connection.on("ReceiveMessage", (message) => {
        if (message.sender !== this.user.userId) {
          this.messages.push(message);
        }
      });
    },
    getMessageContainerClass(sender) {
      if (sender === this.user.userId) return "sender-message";
      return "receiver-message";
    },
    async fetchReceiverUserId() {
      try {
        const response = await axios.get(`${API_BASE}/api/Message/patientConversation/${this.currentConversation}`);
        this.receiverUserId = response.data;
      } catch (error) {
        console.error(error);
      }
    },

    // async fetchConversation() {
    //   try {
    //     const response = await axios.get(`${API_BASE}/api/Message/getConnection/${this.user.userId}/${this.receiverUserId}`);
    //     this.conversationId = response.data.conversationId;
    //   } catch (error) {
    //     console.error(error);
    //   }
    // },
    sendMessage() {
      const message = {
        sender: this.user.userId,
        receiver: this.receiverUserId,
        text: this.messageText,
        imageData: this.selectedImage,
        roomName: this.getWSRoomName(),
        //roomName: this.currentConversation,
        connectionId: this.connection.connectionId,
      };
      axios
          .post(`${API_BASE}/api/Message`, message)
          .then(() => {
            this.messageText = "";
            this.selectedFile = null;
            this.getMessages();
            this.$nextTick(this.scrollToBottom);
          })
          .catch((error) => {
            console.error(error);
          });
    },
    getMessages() {
      console.log("aici")
      console.log(this.user.userId)
      console.log(this.receiverUserId)
      axios
          .get(`${API_BASE}/api/Message/${this.user.userId}/${this.receiverUserId}`)
          .then((response) => {
            this.messages = response.data;
            this.$nextTick(this.scrollToBottom);
            console.log("da")
          })
          .catch((error) => {

            console.error(error);
          });
    },
    async getConversations() {
      try {
        const response = await axios.get(`${API_BASE}/api/Message/patientConversationsReceiverNames/${this.user.userId}`);
        this.conversations = response.data;
        console.log(this.conversations)
        if (this.currentConversation === null) this.currentConversation = this.conversations[0].conversationId;
        console.log(this.currentConversation)
        this.therapists = this.conversations.map(conversation => conversation.therapist);


      } catch (error) {
        console.error(error);
      }
    },

    navigateToConversation(conversationId) {
      console.log(conversationId)
      this.$router.push({path: '/conversations/' + conversationId});
      this.currentConversation = conversationId;


    },
    scrollToBottom() {
      const container = this.$refs.messageContainer;
      if (container) {
        container.scrollTop = container.scrollHeight;
      }
    },
  },
  computed: {
    ...mapState(useUserStore, ['user']),
  },
  watch: {
    '$route.params.id': {
        handler: function(search) {
          this.currentConversation = this.$route.params.id;
          this.initializeChat();
          console.log(search)
        },
        deep: true,
        immediate: true
      }
      },
};
</script>

<style scoped lang="scss">
.page-wrapper {
  width: 100%;
  height: 90vh;
  display: flex;
  flex-direction: row;
}


.message-chat {
  flex: 1;
  background-color: white;
  padding: 15px;
  /* border-radius: 12px; */
  overflow-y: auto; /* Enable vertical scrolling if necessary */
}

.message-content {
  display: flex;
  justify-content: flex-start;
  align-items: center;
  margin-bottom: 10px;
}

.message-content.sent {
  justify-content: flex-end;
}

.text {
  padding: 10px;
  background-color: #f8e896;
  color: #000;
  border-radius: 10px;
  font-size: 14px;
  line-height: 1.5;
}

.text.sent {
  background-color: #A8DDFD;
}

.footer-chat {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-top: auto;
}

.footer-chat form {
  flex: 1;
  display: flex;
}

.footer-chat input[type="text"] {
  flex: 1;
  padding: 8px;
  border: 1px solid #ccc;
  border-radius: 4px;
  margin: 5px;
  color: black;
}

.footer-chat button {
  padding: 8px 16px;
  background-color: #4CAF50;
  color: #fff;
  border: none;
  border-radius: 4px;
  cursor: pointer;
}

.footer-chat button:hover {
  background-color: #45a049;
}

.sender-message {
  display: flex;
  justify-content: flex-end;
}

.receiver-message {
  display: flex;
}

.sender-message .text {
  background-color: #A8DDFD;
  color: #000;
}

.receiver-message .text {
  background-color: #f8e896;
  color: #000;
}

.name-list {
  flex: 0 0 30%;
  margin-right: 10px;
  margin-left: 1px;
}

.paperclip-btn:hover .v-icon {
  color: red; /* Change to your desired hover color */
}

@media (max-width: 960px) {
  .page-wrapper {
    flex-direction: column;
  }
  .name-list {
    flex-basis: auto;
    margin: 0 0 10px 0;
  }
}

</style>

