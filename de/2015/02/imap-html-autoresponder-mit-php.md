---
title: "IMAP-HTML-Autoresponder mit PHP"
date: "2015-02-13"
categories: 
  - "php"
  - "programmierung"
tags: 
  - "autresponder"
  - "imap-de"
  - "php-de"
  - "swiftmailer-de"
---

Jeder kennt spam. Jeder **hasst** spam. Und jeder, der seine E-Mail-Adresse im Internet veröffentlicht (z.B. weil er eine eigene Domain besitzt) bekommt **eine Menge** Spam.

Ich gehöre zu den Leuten, die eine Menge Spam erhalten. Natürlich gibt es Anti-Spam-Lösungen, aber die beste Lösung wäre es, wenn der Spam gar nicht erst ankommt. Und Spamfilter kosten eine Menge Arbeit und/oder filtern zu wenig oder zu viel. Darum habe ich mich entschlossen, einen "Torwächter" in PHP zu schreiben, der möglichst viel Spam abblockt, aber alle gültigen E-Mails durchlässt.

Dazu wird automatisch eine Antwort erstellt. In dieser Antwort ist meine gültige, "geheime" E-Mail-Adresse als Bild hinterlegt. Bots werden dieses Bild nicht lesen können, ein Mensch jedoch schon. Natürlich gibt es auch noch Bots, die tatsächlich durch kommen sollen. Beispielsweise soll der periodische Check meiner Denic-Daten nicht geblockt werden. Hierzu existiert eine simple White-List.

Ich habe mich bewusst dazu entschieden, das Ganze so simpel wie Möglich zu halten. Ein paar Sachen konfigurieren, PHP - Dateien hochladen und fertig. Auf MySQL-Verbindungen o.ä. habe ich also bewusst verzichtet.

Ganz ohne zusätzliche Tools kommt das Script leider nicht aus. Ein Ersatz für die "mail"-Funktion von php war mittels swiftmailer notwendig. Das ist aber kein größeres Problem: Dateien herunterladen und die Inhalte des "lib"-Verzeichnisses auf den Webserver hochladen. Das war es im Wesentlichen schon.

Der Swiftmailer wird durch eine schlichte Zeile eingebunden:

```
require\_once './swift/lib/swift\_required.php';
```

Für den Zugriff auf das Postfach sind ein paar IMAP-Einstellungen notwendig. Theoretisch ginge auch ein POP-Zugriff, der IMAP-Zugriff hat aber klare Vorteile, da dort der E-Mail-Inhalt nicht heruntergeladen werden muss. Und der interessiert uns für den Autoresponder nicht.

```
// IMAP-Settings:
 $hostname = "{imap.hostname.com:993/imap/ssl}INBOX";
 $username = "username";
 $password = "password";
 ```

Nun noch schnell die E-Mail-Adressen konfigurieren. $targetMail ist die echte, private E-Mail-Adresse. $thisMail ist die öffentliche E-Mail-Adresse, die den Spam abfangen soll.

```
$targetMail="private@mydomain.com"; 
$thisMail="webmaster@mydomain.com";
```

Als nächstes muss ein Bild erstellt werden, dass ausschließlich die private E-Mail-Adresse enthält. Sucht eine schöne Schriftart aus. Wer will kann auch eine Captcha-Schrift wählen. Aber Oma ist damit dann vermutlich auch raus...

Lade das Bild nun auf den Webserver und trage die URL in das Script ein:

```
$image="http://www.example.com/images/mail.png";
```

Jetzt sind noch ein paar Texte zu hinterlegen. Und zwar die E-Mail-Inhalte, die der Sender sehen soll. $templateHtml ist für den HTML-Inhalt der mail, $templatePlain ist für diejenigen, die keine HTML-Email anzeigen können (oder wollen). Nur Besitzer von E-Mail-Clients mit HTML-Funktionen sind in der Lage, das Bild anzuzeigen. Die anderen müssen stattdessen auf einen Link klicken.

$templateForward ist zu guter Letzt der Text, den Du selbst siehst, wenn eine E-Mail aufgrund der Whitelist weitergeleitet wird.

Also, sei kreativ :)

```
$templateHtml="<html><body>Thanks for your mail. Please note that this mail is no more. It has ceased to be! It is expired and gone to meet its maker! This is an Ex-Mail!<br/>But don't be afraid, there is a replacement for that. Just send your mail to: <br/><img src=\\"\_\_EMBED\_\_\\"/><br/><br/>If you cannot see that image: Click onto the following link to see my: <a href=\\"\_\_URL\_\_\\">new mail-address</a><br/>And no: This mail will not be forwarded. No one will see this until you forward it to my new mail-address. Thanks!</body></html>";

$templatePlain="Thanks for your mail. Please note that this mail is no more. It has ceased to be! It is expired and gone to meet its maker! This is an Ex-Mail!\\r\\nBut don't be afraid, there is a replacement for that. Just click onto the following link to see my: new mail-address: \_\_URL\_\_";

$templateForward="new message redirected";
```

Zwei spezielle Platzhalter sind erlaubt. "\_\_EMBED\_\_" wird automatisch durch das Bild ersetzt. (nur im HTML-Template möglich)"\_\_URL\_\_" enthält stattdessen einen direkten Link zu der Datei.

Als allerletzte Option kann optional die Whitelist hinterlegt werden. Trenn die gültigen Absender durch Komma.

```
$whitelist = "uberspace,denic.de,domainbox.net,romrobot.com,inwx.de,millerntor.hamburg,dotnet.work";
```

Das war es auch schon mit der Konfiguration. Nun noch etwas Info, was das Script überhaupt macht:

Zunächst prüft das Script, ob auf dem IMAP-Server neue, ungelesene E-Mails liegen. Es werden dann die Header (Betreff, Absender, etc.) der E-Mails abgerufen. Der Absender wird mit der White-List verglichen. Ist der Absender in der Whitelist enthalten, wird der E-Mail-Inhalt heruntergeladen und die E-Mail direkt als Anhang weitergeleitet an Deine private Adresse.

Befindet sich der Absender nicht in der Whitelist, wird stattdessen eine automatische Antwort zurückgeschickt, welche das Bild mit der privaten E-Mail enthält.

Als letzter Schritt wird die E-Mail als gelesen markiert.

In einer perfekten Welt...

... wäre es das jetzt. Aber leider kann (und wird) es Probleme mit Absendern geben, die nicht dem RFC 2822-Standard entsprechen. Die meisten Spammer sind übrigens Standardkonform. Deine Mutti ist es nicht... :)

Das liegt daran, dass es einen E-Mail-Client von einer kleinen Firma in Redmond gibt, die sich mit Standards nicht so auskennt... Das ist zwar unschön, aber in unserem Fall kein wirklich großes Problem. Öffne einfach die Datei _lib\\classes\\swift\\Mime\\Headers\\MailboxHeader.php_ von swiftmailer kommentiere den "throw"-Part der letzten Funktion aus:

```
private function \_assertValidAddress($address)
{
   if (!preg\_match('/^'.$this->getGrammar()->getDefinition('addr-spec').'$/D', $address)) {
     /\* throw new Swift\_RfcComplianceException('Address in mailbox given \['.$address.'\] does not comply with RFC 2822, 3.6.2.');\*/
   }
}
```
 

Das wars dann auch schon. Hier nun das komplette Script:

```
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
```

Speichere es einfach als "reply.php" (oder einen beliebigen anderen Dateinamen) und schick ein paar Test-Emails. Öffne dann das Script in Deinem Browser. Wenn alles soweit in Ordnung ist, kannst Du einen Cronjob eintragen um das Script automatisch auszuführen. Das folgende Beispiel macht dies etwa alle 5 Minuten:

```
\*/5 \* \* \* \* php /var/www/reply.php
```

 

Wenn Du Hilfe brauchst: Einfach kommentieren. Viel Spaß :)
