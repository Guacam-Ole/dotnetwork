---
title: "IMAP-HTML-Autoresponder with PHP"
date: "2015-02-13"
categories: 
  - "development"
  - "php-en"
tags: 
  - "autoresponder"
  - "imap"
  - "php"
  - "redirect"
  - "reply"
  - "spam"
  - "swiftmailer"
---

Everyone knows spam. Everyone **hates** spam. And everyone who has his email-address written on the net (for example in the NIC-records for a domain) gets **a lot of** spam.

I am one of those guys who gets a lot of spam. Of course there are anti-spam solutions out there, but the best would be to not even receive spam. And spam filters need a lot of work and / or do not filter 100% perfect. That is why I decided to use a public email that has to go through a "gatekeeper". A PHP-script that tells everyone that this is not my real mail address, but that there is another one where I could be contacted.

That email-address is not displayed in plain text but as an image to get rid of all those bots. SOME bots however should be able to get through: For example my hoster who needs to validate my email periodically. So these messages just are being forwarded.

I wanted to be as simple as possible. That's why this script does not need much more than plain php. Only a replacement for phps "mail" was needed.

First step: Download swiftmailer. Just put the contents of the "lib" - directory onto your web space. That's it. Ready to go.

Simple include the following line to access swiftmailer:

require\_once './swift/lib/swift\_required.php';

To access your mail you need to enter the IMAP-Settings. POP would work, too, but IMAP would be much more useful because we do not want to download the email-body of the messages processed:

// IMAP-Settings:
 $hostname = "{imap.hostname.com:993/imap/ssl}INBOX";
 $username = "username";
 $password = "password";

Now enter your email-addresses. $targetMail is your real ("hidden") email-address where valid emails should be forwarded to. $thisMail is the public address that receives the spam.

$targetMail="private@mydomain.com"; 
$thisMail="webmaster@mydomain.com";

Next you need to create an image with your favourite picture editing tool (GiMP, Paint.NET, PaintShopPro, etc.) This image should only contain your private email. Take a font you like. You might also use a "Captcha"-font. I - personally - hate that... :)

Upload that image to your web server and enter the url of that image. PNG would be a good choice.

$image="http://www.example.com/images/mail.png";

Next, enter the text the sender should receive. By default everyone receives $templateHtml. For those who do not have the capabilities to display html (or don't want to) a simple text-mail ($templatePlain) will be displayed. Users with a html-browser will see your email-image directly in the mail. Plain-text mail-clienst will display a link to the image instead.

$templateForward defines the mail content YOU will see if a mail is forwarded to you from a valid sender

So it is time to be creative :)

$templateHtml="<html><body>Thanks for your mail. Please note that this mail is no more. It has ceased to be! It is expired and gone to meet its maker! This is an Ex-Mail!<br/>But don't be afraid, there is a replacement for that. Just send your mail to: <br/><img src=\\"\_\_EMBED\_\_\\"/><br/><br/>If you cannot see that image: Click onto the following link to see my: <a href=\\"\_\_URL\_\_\\">new mail-address</a><br/>And no: This mail will not be forwarded. No one will see this until you forward it to my new mail-address. Thanks!</body></html>";

$templatePlain="Thanks for your mail. Please note that this mail is no more. It has ceased to be! It is expired and gone to meet its maker! This is an Ex-Mail!\\r\\nBut don't be afraid, there is a replacement for that. Just click onto the following link to see my: new mail-address: \_\_URL\_\_";

$templateForward="new message redirected";

As you can see there are two special tags in those templates. "\_\_EMBED\_\_" will be replaced with the embedded image (html-template only) and "\_\_URL\_\_" will contain a link to that image.

The last variable you can define is the white-list. Everything entered there is interpreted as a valid sender. Enter multiple values seperated by commas.

$whitelist = "uberspace,denic.de,domainbox.net,romrobot.com,inwx.de,millerntor.hamburg,dotnet.work";

that's all for configuration. Now just some explanation what the script does:

First of all the script checks your IMAP-Server for unread messages. It collects the header of each of those messages and compares the sender with the white-list. If the sender is recognized, the mail body is downloaded and sent as an attachment to the mail address you defined as $targetMail.

If the sender is not in the white-list, an automatic reply is generated using your html-template. It contains the image you downloaded as an attachment and embedded image of the mail. The last step is to mark the mail as read so it won't be processed again.

In a perfect world...

Everything works now. But you can (and will) get problems sending the mail to the sender if the sender's address does not comply with RFC 2822. While most spammers DO comply, mom DOESN'T... :) That's because there is a mail client from a tiny little software company in Redmond, where they do not know much about standards. But this is no big problem at all. Just open _lib\\classes\\swift\\Mime\\Headers\\MailboxHeader.php_ from swiftmailer and put _/\* \*/_ around the throw-part of the last function:

private function \_assertValidAddress($address)
{
   if (!preg\_match('/^'.$this->getGrammar()->getDefinition('addr-spec').'$/D', $address)) {
     /\* throw new Swift\_RfcComplianceException('Address in mailbox given \['.$address.'\] does not comply with RFC 2822, 3.6.2.');\*/
   }
}

 

So finally here is the complete script.

<?php
   require\_once './swift/lib/swift\_required.php';// IMAP-Settings:
   $hostname = "{imap.example.com:993/imap/ssl}INBOX";
   $username = "username";
   $password = "password"; // Script-Settings:
   $targetMail="private@example.com"; // "hidden" target mail-address
   $thisMail="webmaster@example.com"; // "official" mail-address (which receives the spam) 
   $image="http://www.dotnet.work/reply/mail.png"; // Image to be shown
   $templateHtml="<html><body>Thanks for your mail. Please note that this mail is no more. It has ceased to be! It is expired and gone to meet its maker! This is an Ex-Mail!<br/>But don't be afraid, there is a replacement for that. Just send your mail to: <br/><img src=\\"\_\_EMBED\_\_\\"/><br/><br/>If you cannot see that image: Click onto the following link to see my: <a href=\\"\_\_URL\_\_\\">new mail-address</a><br/>And no: This mail will not be forwarded. No one will see this until you forward it to my new mail-address. Thanks!</body></html>";
   $templatePlain="Thanks for your mail. Please note that this mail is no more. It has ceased to be! It is expired and gone to meet its maker! This is an Ex-Mail!\\r\\nBut don't be afraid, there is a replacement for that. Just click onto the following link to see my: new mail-address: \_\_URL\_\_";
   $templateForward="new message redirected";

   $whitelist = "uberspace,denic.de,domainbox.net,romrobot.com,inwx.de,millerntor.hamburg,dotnet.work";
 
   $allowedSenders=explode(",",$whitelist);
   $inbox = imap\_open($hostname,$username,$password) or die("Sorry, connection failed");
   $emails = imap\_search($inbox,"UNSEEN");
   $count=0; 
   echo "<pre>"; 
   if($emails) {
     $transport = Swift\_MailTransport::newInstance();
     foreach($emails as $email) {
        $count++;
        $header = imap\_fetch\_overview($inbox,$email,0); 
        $subject=$header\[0\]->subject;
        $sender=$header\[0\]->from;

        $messageHeader =  imap\_fetchheader($inbox, 1)
        if (strpos($messageHeader,"X-RESPONDERDATE")!==false) {
          continue; // no wild bouncing, please
        }

        if (strpos($sender,$targetMail!==false || strpos($sender,$thisMail!==false) {
           // We want no loops... :)
           continue;
        }
        echo "retrieving ".$subject." from ".$sender."\\r\\n";
        // Whitelist: 
        $isWhiteListed=false;
      
        foreach($allowedSenders as $allowedSender) {
           if (stripos($sender,$allowedSender)!==false) {
              $isWhiteListed=true;
              break;
           }
        }
 
        if ($isWhiteListed) {
           $email\_body = imap\_body($inbox, $email);
           $mailer = Swift\_Mailer::newInstance($transport);
           $message = Swift\_Message::newInstance()
              ->setSubject("Fw:".$subject)
              ->setFrom($sender)
              ->setTo($targetMail)
              ->setBody($templateForward)
           ;
 
           $attachment = Swift\_Attachment::newInstance($email\_body, 'origin.eml', 'message/rfc822'); 
           $message->attach($attachment);
 
           $result = $mailer->send($message);
           echo "whitelist redirect\\r\\n";
        } else { 
           $mailer = Swift\_Mailer::newInstance($transport);
           $message = Swift\_Message::newInstance();
           $swiftheaders = $message->getHeaders();
           $swiftheaders->addTextHeader('X-RESPONDERDATE', date('Y-m-d H:i:s'));
           $cid = $message->embed(Swift\_Image::fromPath($image));
           $html=str\_replace("\_\_URL\_\_",$image,$templateHtml);
           $html=str\_replace("\_\_EMBED\_\_",$cid,$html);
           $plain=str\_replace("\_\_URL\_\_",$image,$templatePlain);
           $message
              ->setSubject("Re:".$subject)
              ->setFrom($thisMail)
              ->setTo($sender) 
              ->setBody($html, 'text/html')
              ->addPart($plain, 'text/plain')
           ;
           $result = $mailer->send($message);
           echo "answered\\r\\n";
        } 
        imap\_setflag\_full($inbox, imap\_uid($inbox,$email), "\\\\Seen \\\\Flagged", ST\_UID);
     } 
  }
  imap\_close($inbox);
  echo "done redirecting ".$count." mails\\r\\n";
  echo "</pre>";
?>

 

Just save this program as "reply.php", send test-mails and open "reply.php" in your browser to test the settings. When everything works fine you can simply add a cronjob so this script is run automaticly. The following example runs the script every 5 minutes:

\*/5 \* \* \* \* php /var/www/reply.php

 

Just use the comments if you need any help
