<template>
  <div class="page">
    <PageTitleBar title="Conversații"/>
    <div class="page-wrapper">
  
      <v-card class="name-list">
      
          <v-list>
              <v-list-item v-for="patient in patients" 
              :key="patient.id" 
              :value="patient.id" 
              @click="navigateToConversation(patient.id)">
                <v-list-item-content>
                  <v-list-item-title>{{ patient.firstName }} {{ patient.lastName }}</v-list-item-title>
                </v-list-item-content>
              </v-list-item>
        </v-list>

      
      </v-card>
      <v-card class="message-chat" style="max-height: 100%; overflow-y: hidden; display:flex; flex-direction: column; variant:outlined">
        <div style="overflow-y: scroll; height:0; flex-grow: 1;" ref="messageContainer">
          <div class="messages">
            <div v-for="(message, index) in messages" :key="index" :class="getMessageContainerClass(message)">
              <div class="message-content">
                <p class="text">{{ message.text }}</p>
              </div>
            </div>
        </div>
        <div style="height: 50px;" id="scroll-target"></div>
      </div>
        <div class="footer-chat">
        
          <v-form @submit.prevent="sendMessage" class="input-container">
            <input type="text" placeholder="Type your message here" v-model="messageText"/>
            <!-- <input type="file" @change="handleFileUpload" /> -->
            <v-btn type="submit" color="primary">Send</v-btn>
          </v-form>
        </div>
      </v-card>
      
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
      messageText: "",
      messages: [],
      connection: null,
      conversationId: null,
      receiverTableId: this.$route.params.id,
      receiverUserId: null,
      patients: []
    };
  },
  mounted() {
    this.initializeChat();
    this.fetchPatients ();
  },
  methods: {
    async initializeChat() {
      await this.fetchReceiverUserId();
      await this.fetchConversation();
      this.startConnection();
    },
    getWSRoomName() {
      return this.conversationId;
    },
    startConnection() {
      this.connection = new signalR.HubConnectionBuilder().withUrl(`${API_BASE}/chatHub`).build();

      this.connection
        .start()
        .then(() => {
          this.getMessages(); // Fetch messages after connection is started
        })
        .catch((error) => {
          console.error(error);
        });

      this.connection.on("ReceiveMessage", (message) => {
        this.messages.push(message);
      });
    },
    getMessageContainerClass(message) {
      if (message.sender === this.user.userId) {
        return "sender-message";
      }
      return "receiver-message";
    },
    async fetchReceiverUserId() {
      try {
        const response = await axios.get(`${API_BASE}/api/Message/getUserId/${this.$route.params.id}/Patient`);
        this.receiverUserId = response.data;
      } catch (error) {
        console.error(error);
      }
    },
    async fetchConversation() {
      try {
        const response = await axios.get(`${API_BASE}/api/Message/getConnection/${this.user.userId}/${this.receiverUserId}`);
        this.conversationId = response.data.conversationId;
      } catch (error) {
        console.error(error);
      }
    },
    async fetchPatients () {
      try {
        const response = await axios.get(`${API_BASE}/api/Therapist/${this.user.tableUserId}/patients`);
        this.patients = response.data;
        console.log(response.data)
      } catch (error) {
        console.error(error);
      }
    },
    sendMessage() {
      const message = {
        sender: this.user.userId,
        receiver: this.receiverUserId,
        text: this.messageText,
        imageData:'',
        roomName: this.getWSRoomName(),
        connectionId: this.connection.connectionId,
      };

      axios
        .post(`${API_BASE}/api/Message`, message)
        .then(() => {
          // Message sent successfully, no need to push it again
        })
        .catch((error) => {
          console.error(error);
        });

      this.messageText = ""; // Clear the input field
      this.$nextTick(this.scrollToBottom);
    },
    getMessages() {
      axios
        .get(`${API_BASE}/api/Message/${this.user.userId}/${this.receiverUserId}`)
        .then((response) => {
          this.messages = response.data;
          this.$nextTick(this.scrollToBottom);
        })
        .catch((error) => {
          console.error(error);
        });
    },
    navigateToConversation(patientId) {
    this.$router.push({ path: '/therapist/patients/' + patientId + '/conversation' });
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
    currentUser() {
      return localStorage.getItem("userid");
    },
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
  
  padding: 15px;
  
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
  /* background-color: #A8DDFD; */
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
.patient-name {
  text-decoration: none;
  color: inherit;
  cursor: auto;
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
