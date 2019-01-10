package cl.fuentes.rabbitmq.test;

import com.rabbitmq.client.ConnectionFactory;
import com.rabbitmq.client.Connection;
import com.rabbitmq.client.Channel;

public class Send{

    private final static String QUEUE_NAME = "MICOLA";
    
    public static void main(String[] argv) throws Exception {
    
        ConnectionFactory factory = new ConnectionFactory();
        factory.setHost("localhost");
        try (Connection connection = factory.newConnection();
            
            Channel channel = connection.createChannel()) {
            channel.queueDeclare(QUEUE_NAME, false, false, false, null);
            
            //String message = "Mensaje de cola";
            String message = "";

            for(int i = 1 ; i <=100 ; i++){
                message = "Mensaje N : " + i;
               channel.basicPublish("", QUEUE_NAME, null, message.getBytes("UTF-8"));
               System.out.println(" [x] Enviado '" + message + "'");
            }
        }
    }
}