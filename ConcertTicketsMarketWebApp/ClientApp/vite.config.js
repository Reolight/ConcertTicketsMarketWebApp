import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'
import mkcert from 'vite-plugin-mkcert'

import { env } from 'process';

import fs from 'fs';
import path from 'path'
import { execSync } from 'child_process';

const target = env.ASPNETCORE_HTTPS_PORT ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}` :
  env.ASPNETCORE_URLS ? env.ASPNETCORE_URLS.split(';')[0] : 'http://localhost:10372';

console.log(target);

// https://vitejs.dev/config/
export default defineConfig({
	server: {
		https: generateCerts(),
		//strictPort: true,
		port: 44419,
		proxy: { 
			target: {
				secure: false,
				target: target,
				changeOrigin: true
			}
		}
	},
  	plugins: [react()]
})


function generateCerts() {
    const baseFolder =
        process.env.APPDATA !== undefined && process.env.APPDATA !== ""
            ? `${process.env.APPDATA}/ASP.NET/https`
            : `${process.env.HOME}/.aspnet/https`;
    const certificateArg = process.argv
        .map((arg) => arg.match(/--name=(?<value>.+)/i))
        .filter(Boolean)[0];
    const certificateName = certificateArg
        ? certificateArg.groups.value
        : process.env.npm_package_name;

    if (!certificateName) {
        console.error(
            "Invalid certificate name. Run this script in the context of an npm/yarn script or pass --name=<<app>> explicitly."
        );
        process.exit(-1);
    }

    const certFilePath = path.join(baseFolder, `${certificateName}.pem`);
    const keyFilePath = path.join(baseFolder, `${certificateName}.key`);

    if (!fs.existsSync(certFilePath) || !fs.existsSync(keyFilePath)) {
        const outp = execSync(
            "dotnet " +
                [
                    "dev-certs",
                    "https",
                    "--export-path",
                    certFilePath,
                    "--format",
                    "Pem",
                    "--no-password",
                ].join(" ")
        );
        console.log(outp.toString());
    }

    return {
        cert: fs.readFileSync(certFilePath, "utf8"),
        key: fs.readFileSync(keyFilePath, "utf8"),
    };
}